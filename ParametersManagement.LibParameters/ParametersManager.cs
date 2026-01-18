using System;
using System.IO;
using Newtonsoft.Json;
using SystemTools.SystemToolsShared;

namespace ParametersManagement.LibParameters;

public sealed class ParametersManager : IParametersManager
{
    private readonly string? _encKey;

    private string? _paramsJsonText;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ParametersManager(string? parametersFileName, IParameters parameters, string? encKey = null)
    {
        ParametersFileName = parametersFileName;
        _encKey = encKey;
        Parameters = parameters;
    }

    public string? ParametersFileName { get; private set; }

    public IParameters Parameters { get; set; }

    public void Save(IParameters parameters, string? message = null, string? saveAsFilePath = null)
    {
        if (!string.IsNullOrWhiteSpace(saveAsFilePath))
            ParametersFileName = saveAsFilePath;

        if (!parameters.CheckBeforeSave())
            StShared.WriteWarningLine("Something wrong with data for save", true, null, true);

        _paramsJsonText = JsonConvert.SerializeObject(parameters, Formatting.Indented);

        //იშიფრება მთლიანი json ტექსტი. ამის გამო შეუძლებელია მანქანებს შორის სეთინგების გადატანა
        //FIXME შესაბამისად შესაძლებელი უნდა იყოს გაშიფრული ვარიანტის შენახვა
        if (_encKey != null)
            _paramsJsonText = EncryptDecrypt.EncryptString(_paramsJsonText, _encKey);

        var filePathForSave = !string.IsNullOrWhiteSpace(saveAsFilePath) ? saveAsFilePath : ParametersFileName;

        //FIXME მიმდინარე ფაილის შეცვლამდე უნდა მოხდეს ბექაპის დამახსოვრება
        //დავადგინოთ შესანახი ფაილის სახელის მიხედვით არსებობს თუ არა ფაილი.
        //თუ ფაილი არსებობს, მისი სახელის მიხედვით დაგენერირდეს bak ფაილის სახელი არსებული ფაილის სახელის გამოყენებით. უნდა დაემატოს თარიღი და გაფართოება .bak
        //შევუცვალოთ სახელი არსებულ ფაილს დაგენერირებული ფაილის მიხედვით

        if (string.IsNullOrWhiteSpace(filePathForSave)) throw new Exception("filePathForSave is empty, cannot save");

        //შევინახოთ პარამეტრების ფაილი
        File.WriteAllText(filePathForSave, _paramsJsonText);

        //დავადგინოთ არსებობს თუ არა ძალიან ძველი bak ფაილები და წავშალოთ

        Parameters = parameters;
        if (string.IsNullOrWhiteSpace(message))
            return;

        StShared.WriteSuccessMessage(message);
    }
}