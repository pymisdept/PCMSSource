using System;
using System.Collections.Generic;
using System.Text;

namespace PCCore
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
        void ClearError();
    }

}
