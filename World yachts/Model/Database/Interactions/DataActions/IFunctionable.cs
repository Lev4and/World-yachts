using System;
using World_yachts.Model.Database.Models;

namespace World_yachts.Model.Database.Interactions.DataActions
{
    public interface IFunctionable
    {
        bool NecessaryChagePassword(DateTime dateOfLastChangePassword);

        bool NecessaryChagePassword(User user);

        bool NecessaryChagePassword(v_user user);
    }
}
