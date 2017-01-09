using Assimil.Domain;

namespace Assimil.Core
{
    public interface IAssimil
    {
        PagingAssimil Get(int page, int pageSize);
    }
}
