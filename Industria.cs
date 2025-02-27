public class Industria
{
    public int Emprega { get; set; }
    public float Salario { get; set; }
    public float CaixaInicial { get; set; }
    public float CustoProducaoItem { get; set; }
    public float PrecoVenda { get; set; }

    public override string ToString()
    {
        return $"Indústria -> Emprega: {Emprega}, Salário: {Salario}, Caixa: {CaixaInicial}, Preço: {PrecoVenda}, Custo Produção: {CustoProducaoItem}";
    }
}
