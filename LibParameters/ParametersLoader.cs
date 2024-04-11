using System;
using System.IO;
using Newtonsoft.Json;
using SystemToolsShared;

namespace LibParameters;

public sealed class ParametersLoader<T> where T : class, IParameters, new()
{
    private readonly string? _encKey;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ParametersLoader(string? encKey = null)
    {
        _encKey = encKey;
    }

    public IParameters? Par { get; private set; }
    public string? ParametersFileName { get; set; }


    public bool TryLoadParameters(string paramsFileName)
    {
        Par = null;
        try
        {
            // ReSharper disable once using
            using StreamReader reader = new(paramsFileName);

            var json = reader.ReadToEnd();
            string? decryptedJson = null;
            if (_encKey != null)
                decryptedJson = EncryptDecrypt.DecryptString(json, _encKey);

            if (decryptedJson != null)
                json = decryptedJson;

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };


            var parameters = JsonConvert.DeserializeObject<T>(json, settings);
            reader.Close();
            Par = parameters;
            return true;
        }
        catch (Exception e)
        {
            StShared.WriteException(e, true);
        }

        return false;
    }
}