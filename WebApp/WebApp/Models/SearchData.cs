using WebApp.Models.Documents;

namespace WebApp.Models
{
    public class SearchData : BaseDocument
    {
        private bool successful;
        private string information;


        public SearchData()
        {
            successful = false;
            information = "not checked";

        }

        public bool Successful { get => successful;}

        public string Information { get => information; }


        public void ValidateInput()
        {
            successful = false;
            information = "error";
        }
    }
}