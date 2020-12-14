using BoxesPojectShared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesPojectShared.Interfaces
{
    public interface IUserInteractions
    {
        bool RequestConfirmation(List<Box> obj);
    }
}
