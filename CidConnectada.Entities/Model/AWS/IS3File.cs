namespace CidConnectada.Entities.AWS
{
    public interface IS3File
    {
        string GetS3Key(string extensao = null);
        string GetS3Url(string baseUrl, string extensao);

    }
}
