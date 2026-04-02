namespace ControleFiscal.Domain.DTO.ControleFiscal
{
    public class RetornoPaginado<T>
    {
        public int QtdRegistro { get; set; }
        public IList<T>? Lista { get; set; }

    }
}
