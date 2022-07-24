namespace Blogvio.WebApi.Seetings;

public class JWT
{
	public string Key { get; set; }
	public string Issuer { get; set; }
	public string Audience { get; set; }
	public TimeSpan TokenLifeTime { get; set; }
}
