using System.Collections.Generic;

namespace MVC.Data.Converter.Contract
{
    public interface IParser<O, D>
    {
        D Parse(O origin);

        List<D> Parse(List<O> orgins);
    }
}
