using System.ComponentModel.DataAnnotations;

public class Devices
{
    [Key]
    public string DeviceId { get; set; }

    public string Name { get; set; }

    public string Location { get; set; }

    public string Type { get; set; }

    public string AssetId { get; set; }



}