# TestDapper
Dapper / Contrib test repo

## Exemplo com Procedure

```sql
CREATE PROCEDURE dbo.PRC_SEL_DETALHES_ESTADO
(
	@CodEstado char(2)
)
AS
BEGIN

	SELECT E.SiglaEstado, E.NomeEstado, E.NomeCapital,
	       R.NomeRegiao
	FROM dbo.Estados E
	INNER JOIN dbo.Regioes R ON R.IdRegiao = E.IdRegiao
	WHERE E.SiglaEstado = @CodEstado
END
```

```c#
// exemplo buscando de uma stored procedure
public Estado GetDetalhesEstadoProcedure(string siglaEstado)
{
    using (SqlConnection conexao = new SqlConnection(
        _config.GetConnectionString("ExemplosDapper")))
    {
        return conexao.QueryFirstOrDefault<Estado>(
            "dbo.PRC_SEL_DETALHES_ESTADO",
            new
            {
                CodEstado = siglaEstado
            },
            commandType: CommandType.StoredProcedure);
    }
}
```
