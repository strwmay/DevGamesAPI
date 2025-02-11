using System.ComponentModel.DataAnnotations;

namespace devGamesAPI.Models
{
    public class ItemCarrinho
    {
        public Guid ItemCarrinhoId { get; set; }
        public Guid CarrinhoId { get; set; }
        public Guid JogoId { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        [Range(0.01, 999.99, ErrorMessage = "O valor deve ser maior que zero")]
        public decimal Valor { get; set; }
        public decimal? Total { get; set; }
    }
}
