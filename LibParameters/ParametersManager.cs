using System;
using System.IO;
using Newtonsoft.Json;
using SystemToolsShared;

namespace LibParameters;

public sealed class ParametersManager : IParametersManager
{
    private readonly string? _encKey;

    public ParametersManager(string? parametersFileName, IParameters parameters, string? encKey = null)
    {
        ParametersFileName = parametersFileName;
        _encKey = encKey;
        Parameters = parameters;
    }

    //public ParametersManager(IParameters parameters, string? encKey = null)
    //{
    //    ParametersFileName = null;
    //    _encKey = encKey;
    //    Parameters = parameters;
    //}

    public string? ParamsJsonText { get; set; }
    public string? ParametersFileName { get; set; }
    public IParameters Parameters { get; set; }

    public void Save(IParameters parameters, string message, bool pauseAfterMessage = true,
        string? saveAsFilePath = null)
    {
        if (!string.IsNullOrWhiteSpace(saveAsFilePath))
            ParametersFileName = saveAsFilePath;

        if (!parameters.CheckBeforeSave())
            StShared.WriteWarningLine("Something wrong with data for save", true, null, true);

        ParamsJsonText = JsonConvert.SerializeObject(parameters, Formatting.Indented);

        //იშიფრება მთლიანი json ტექსტი. ამის გამო შეუძლებელია მანქანებს შორის სეთინგების გადატანა
        //FIXME შესაბამისად შესაძლებელი უნდა იყოს გაშიფრული ვარიანტის შენახვა
        if (_encKey != null)
            ParamsJsonText = EncryptDecrypt.EncryptString(ParamsJsonText, _encKey);

        var filePathForSave = !string.IsNullOrWhiteSpace(saveAsFilePath) ? saveAsFilePath : ParametersFileName;

        //FIXME მიმდინარე ფაილის შეცვლამდე უნდა მოხდეს ბექაპის დამახსოვრება
        //დავადგინოთ შესანახი ფაილის სახელის მიხედვით არსებობს თუ არა ფაილი.
        //თუ ფაილი არსებობს, მისი სახელის მიხედვით დაგენერირდეს bak ფაილის სახელი არსებული ფაილის სახელის გამოყენებით. უნდა დაემატოს თარიღი და გაფართოება .bak
        //შევუცვალოთ სახელი არსებულ ფაილს დაგენერირებული ფაილის მიხედვით

        if (string.IsNullOrWhiteSpace(filePathForSave)) throw new Exception("filePathForSave is empty, cannot save");

        //შევინახოთ პარამეტრების ფაილი
        File.WriteAllText(filePathForSave, ParamsJsonText);

        //დავადგინოთ არსებობს თუ არა ძალიან ძველი bak ფაილები და წავშალოთ


        Parameters = parameters;
        if (string.IsNullOrWhiteSpace(message))
            return;
        StShared.WriteSuccessMessage(message);
        if (pauseAfterMessage)
            StShared.Pause();
    }
}