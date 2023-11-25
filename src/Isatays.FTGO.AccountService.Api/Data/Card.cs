using System.ComponentModel.DataAnnotations.Schema;

namespace Isatays.FTGO.AccountService.Api.Data;

[Table(name: "Card", Schema = "AccountService")]
public class Card : Entity
{
    public string CardNumber { get; set; } = string.Empty;

    public DateTime ExpirationDate { get; set; }

    public int CardCode { get; set; }
}
