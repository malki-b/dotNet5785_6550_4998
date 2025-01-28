using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IAdmin
{
    void InitializeDB();
    void ResetDB();
    int GetMaxRange();
    void SetMaxRange(int maxRange);
    DateTime GetClock();
    void ForwardClock(BO.TimeUnit unit);
}
