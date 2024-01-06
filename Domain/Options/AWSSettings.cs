namespace Domain.Options;

public class AWSSettings
{
    public string UserPoolId { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string UserAccessKeyId { get; set; }
    public string UserSecretAccessKey { get; set; }
    public string RegionName { get; set; }
    public Bucket Bucket { get; set; }
}

public class Bucket
{
    public string Name { get; set; }
    public Folders Folders { get; set; }
}

public class Folders
{
    
}
