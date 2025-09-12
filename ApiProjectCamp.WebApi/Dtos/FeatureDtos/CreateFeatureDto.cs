namespace ApiProjectCamp.WebApi.Dtos.FeatureDtos
{
	public class CreateFeatureDto
	{
		public string Title { get; set; }
		public string SubTitle { get; set; }
		public int Description { get; set; }
		public string VideoUrl { get; set; }
		public string ImageUrl { get; set; }
	}
}
