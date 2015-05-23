namespace VkNet.Categories.Extended
{
    /// <summary>
    /// Расширения категории служебных методов.
    /// </summary>
    public class UtilsCategoryExtended
    {
        private readonly VkApi _vk;
        private readonly UtilsCategory _utils;

        internal UtilsCategoryExtended(UtilsCategory utilsCategory, VkApi vk)
        {
            _utils = utilsCategory;
            _vk = vk;
        }
    }
}