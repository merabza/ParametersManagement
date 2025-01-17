using LibParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using SystemToolsShared;

namespace LibFileParameters.Models;

public sealed class SmartSchema : ItemData
{
    public int LastPreserveCount { get; init; }
    public List<SmartSchemaDetail> Details { get; init; } = [];

    public List<DateTime> GetPreserveFileDates(List<BuFileInfo> files)
    {
        if (Details.Count < 1)
            return [];

        //თავიდან ბოლო შექმნილები დავარეზერვოთ
        var preserveDates = files.Select(s => s.FileDateTime).OrderByDescending(obd => obd)
            .Take(LastPreserveCount < 1 ? 1 : LastPreserveCount).ToList();

        //დავადგინოთ რომელია ყველაზე პატარა პერიოდის შესაბამისი დეტალი
        var minPeriodType = Details.Max(m => m.PeriodType);
        var minPeriodSmartSchemaDetail = Details.SingleOrDefault(s => s.PeriodType == minPeriodType);
        if (minPeriodSmartSchemaDetail == null)
            return [];

        //ყველაზე პატარა დეტალისათვის გადავინახოთ PreserveCount რაოდენობის ფაილი
        preserveDates.AddRange(files.Select(s => s.FileDateTime).OrderByDescending(od => od)
            .Take(minPeriodSmartSchemaDetail.PreserveCount));

        //ყველაზე პატარა დეტალისათვის გადავინახოთ PreserveCount რაოდენობის პერიოდში არსებული ფაილი
        preserveDates.AddRange(files.Select(s => s.FileDateTime).Where(w =>
            w >= DateTime.Now.StartOfPeriod(minPeriodSmartSchemaDetail.PeriodType)
                .DateAdd(minPeriodSmartSchemaDetail.PeriodType, -minPeriodSmartSchemaDetail.PreserveCount)));


        //განვიხილოთ ყველაზე პატარა დეტალის გარდა ყველა დანარჩენი
        foreach (var res in Details.Where(w => w.PeriodType != minPeriodType).Select(
                     smartSchemaDetail =>
                         files.GroupBy(g => g.FileDateTime.StartOfPeriod(smartSchemaDetail.PeriodType),
                                 g => g.FileDateTime,
                                 (s, d) => new { Key = s, Min = d.Min() }).Reverse()
                             .Take(smartSchemaDetail.PreserveCount)
                             .Select(s => s.Min)))
            preserveDates.AddRange(res);

        return preserveDates.Distinct().ToList();
    }


    public List<BuFileInfo> GetFilesForDeleteBySchema(List<BuFileInfo> files)
    {
        var preserveDates = GetPreserveFileDates(files);

        return files.Where(buFileInfo => !preserveDates.Contains(buFileInfo.FileDateTime)).ToList();
    }
}