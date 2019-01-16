using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Pages
{
    public interface IPage
    {
        string Title { get; }
        string Url { get; }
        // T NavigateTo<T>(string path) where T : IPage;

        void WaitLoading();

        void AcceptDialog();

        void CancelDialog();

        void BrowserRefresh();

        void Close();

        void Quit();
    }
}
