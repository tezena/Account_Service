using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace AccountService.Models
{
  
    
        [CollectionName("Users")]
        public class ApplicationUser : MongoIdentityUser<Guid>
        {

        }
    
}
