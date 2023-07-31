namespace IntrumWebApi.Configuration
{
    /// <summary>
    /// Объект настроек приложений
    /// <br/>Сюда добавляем поля, если правится application.json
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Секретный ключ, который используется при создании JWT для авторизованного доступа
        /// </summary>
        public string Secret { get; set; } = string.Empty;
    }
}
