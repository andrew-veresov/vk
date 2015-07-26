using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VkNet.Categories
{
    public abstract class BaseCategory
    {
        protected readonly VkApi _vk;

        protected BaseCategory(VkApi vk)
        {
            _vk = vk;
        }
    }
}
