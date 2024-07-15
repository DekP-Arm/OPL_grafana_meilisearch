using VaultSharp;
using VaultSharp.V1.Commons;

public class VaultService
{
    private readonly IVaultClient _vaultClient;

    public VaultService(IVaultClient vaultClient)
    {
        _vaultClient = vaultClient;
    }

    public async Task<Secret<SecretData>> GetSecretAsync(string path)
    {
        return await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path, mountPoint: "secret");
    }
}