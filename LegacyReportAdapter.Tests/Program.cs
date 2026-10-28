using System;
using System.IO;
using System.Collections.Generic;
using LegacyReportAdapter;

var failed = new List<string>();

void Check(string name, bool condition)
{
    if (condition == true)
    {
        Console.WriteLine("PASS - " + name);
    }
    else
    {
        Console.WriteLine("FAIL - " + name);
        failed.Add(name);
    }
}

var output = new StringWriter();
var original = Console.Out;

try
{
    Console.SetOut(output);
    LegacyReportPrinter p = new LegacyReportPrinter();
    IReportExporter exporter = new LegacyReportAdapter.LegacyReportAdapter(p);

    ReportDocument doc = new ReportDocument("Test title", "Test body");
    exporter.Export(doc);
}
finally
{
    Console.SetOut(original);
}

var text = output.ToString();

Check("заголовок передан legacy-принтеру", text.Contains("Test title"));
Check("тело передано legacy-принтеру", text.Contains("Test body"));
Check("сохранён завершающий маркер", text.Contains("END") || text.Contains("REPORT"));

//тест 1: Пустой заголовок
try
{
    LegacyReportPrinter p = new LegacyReportPrinter();
    IReportExporter exporter = new LegacyReportAdapter.LegacyReportAdapter(p);
    ReportDocument badDoc = new ReportDocument("", "Тело отчета");

    exporter.Export(badDoc);
    Check("тест пустого заголовка", false);
}
catch (ArgumentException)
{
    Check("тест пустого заголовка", true);
}

// тест 2: Null документ
try
{
    LegacyReportPrinter p = new LegacyReportPrinter();
    IReportExporter exporter = new LegacyReportAdapter.LegacyReportAdapter(p);

    exporter.Export(null);
    Check("тест null-документа", false);
}
catch (ArgumentNullException)
{
    Check("тест null-документа", true);
}

//тест 3: Null в конструкторе
try
{
    LegacyReportAdapter.LegacyReportAdapter badAdapter = new LegacyReportAdapter.LegacyReportAdapter(null);
    Check("тест null-принтера в конструкторе", false);
}
catch (ArgumentNullException)
{
    Check("тест null-принтера в конструкторе", true);
}

Console.WriteLine("\nПровалено проверок: " + failed.Count);
