using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPressPCL.Models;

namespace WPManager.Models
{
    public interface IWPParameterM
    {
        public string EndpointUri {  get; set; }

        public string Password {  get; set; }

        public string UserName { get; set; }

        public Task CreateOrUpdatePost(int postid, string title, string description, string excerpt, string content, string slug);

        public Task<ObservableCollection<Post>> GetPost();
    }
}
