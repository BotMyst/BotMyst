namespace BotMyst.Shared.Models
{
    public class OptionDescription
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public OptionTypes OptionType { get; set; }
        public object Value { get; set; }
    }
}