using System.Collections.Generic;
using Tel.Egram.Services.Persistance.Resources;

namespace Tel.Egram.Services.Persistance
{
    public interface IResourceManager
    {
        IList<PhoneCode> GetPhoneCodes();
    }
}