//Record -> db de tanımlanmayacak User'a ek veri gibi, DTO ile benzer kullanım.(Value Object)
public sealed record Address
{
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Town { get; set; }
    public string? FullAddress { get; set; }
} 
