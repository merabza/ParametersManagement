using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SystemTools.SystemToolsShared;

namespace ParametersManagement.LibParameters;

public class ParametersManager : IParametersManager
{
    public ParametersManager(IOptions<MainParametersManagerOptions> options)
    {
        ParametersFileName = options.Value.ParametersFileName;
        Parameters = options.Value.Par;
    }

    public ParametersManager(string? parametersFileName, IParameters parameters)
    {
        ParametersFileName = parametersFileName;
        Parameters = parameters;
    }

    public string? ParametersFileName { get; private set; }

    public IParameters Parameters { get; set; }

    public async ValueTask<bool> Save(IParameters parameters, string? message, string? saveAsFilePath = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(saveAsFilePath))
        {
            ParametersFileName = saveAsFilePath;
        }

        if (!parameters.CheckBeforeSave())
        {
            StShared.WriteWarningLine("Something wrong with data for save", true, null, true);
            return false;
        }

        string paramsJsonText = JsonConvert.SerializeObject(parameters, Formatting.Indented);

        //იშიფრება მთლიანი json ტექსტი. ამის გამო შეუძლებელია მანქანებს შორის სეთინგების გადატანა
        //შესაბამისად შესაძლებელი უნდა იყოს გაშიფრული ვარიანტის შენახვა
        //if (_encKey != null)
        //{
        //    paramsJsonText = EncryptDecrypt.EncryptString(paramsJsonText, _encKey);
        //}

        string? filePathForSave = !string.IsNullOrWhiteSpace(saveAsFilePath) ? saveAsFilePath : ParametersFileName;

        //მიმდინარე ფაილის შეცვლამდე უნდა მოხდეს ბექაპის დამახსოვრება
        //დავადგინოთ შესანახი ფაილის სახელის მიხედვით არსებობს თუ არა ფაილი.
        //თუ ფაილი არსებობს, მისი სახელის მიხედვით დაგენერირდეს bak ფაილის სახელი არსებული ფაილის სახელის გამოყენებით. უნდა დაემატოს თარიღი და გაფართოება .bak
        //შევუცვალოთ სახელი არსებულ ფაილს დაგენერირებული ფაილის მიხედვით

        if (string.IsNullOrWhiteSpace(filePathForSave))
        {
            StShared.WriteWarningLine("filePathForSave is empty, cannot save", true, null, true);
            return false;
        }

        //შევინახოთ პარამეტრების ფაილი
        await File.WriteAllTextAsync(filePathForSave, paramsJsonText, cancellationToken);

        //დავადგინოთ არსებობს თუ არა ძალიან ძველი bak ფაილები და წავშალოთ

        Parameters = parameters;
        if (string.IsNullOrWhiteSpace(message))
        {
            return true;
        }

        StShared.WriteSuccessMessage(message);
        return true;
    }
}
