namespace VkNet.Categories.Extended
{
    /// <summary>
    /// Расширения категории работы со статусом пользователя или сообщества.
    /// </summary>
    public class StatusCategoryExtended
    {
        private readonly VkApi _vk;
        private readonly StatusCategory _status;

        internal StatusCategoryExtended(StatusCategory statusCategory, VkApi vk)
        {
            _status = statusCategory;
            _vk = vk;
        }
    }
}