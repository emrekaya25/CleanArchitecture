public sealed record PersonalInformation
{
    public string TcNo { get; set; } = default!; // kullanıcı duplicate olmaması için TcNo üzerinden işlem yapacağız bu yüzden null gelmemesi gerekiyor.
    public string? Email { get; set; }
    public string? Phone { get; set; }
}
