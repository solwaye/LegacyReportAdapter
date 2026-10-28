using System;
using LegacyReportAdapter;

var legacyPrinter = new LegacyReportPrinter();

IReportExporter exporter = new LegacyReportAdapter.LegacyReportAdapter(legacyPrinter);

var document = new ReportDocument(
    "состояние оборудования",
    "всего позиций: 10\nВ ремонте: 3\nСписано: 1"
);

exporter.Export(document);
