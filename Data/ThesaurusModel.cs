using Newtonsoft.Json;

namespace BotMyst.Data
{
    public partial class ThesaurusData
    {
        [JsonProperty ("metadata")]
        public DictionaryMetadata Metadata { get; set; }

        [JsonProperty ("results")]
        public ThesaurusResult [] Results { get; set; }
    }

    public partial class ThesaurusEntry
    {
        [JsonProperty ("notes")]
        public DictionaryCrossReference [] Notes;

        [JsonProperty ("grammaticalFeatures")]
        public DictionaryGrammaticalFeature [] GrammaticalFeatures;

        [JsonProperty ("etymologies")]
        public string [] Etymologies;

        [JsonProperty ("homographNumber")]
        public string HomographNumber;

        [JsonProperty ("senses")]
        public ThesaurusSense [] Senses;

        [JsonProperty ("pronunciations")]
        public DictionaryPronunciation [] Pronunciations;

        [JsonProperty ("variantForms")]
        public DictionaryVariantForm [] VariantForms;
    }

    public partial class ThesaurusResult
    {
        [JsonProperty ("language")]
        public string Language;

        [JsonProperty ("pronunciations")]
        public DictionaryPronunciation [] Pronunciations;

        [JsonProperty ("id")]
        public string Id;

        [JsonProperty ("lexicalEntries")]
        public ThesaurusLexicalEntry [] LexicalEntries;

        [JsonProperty ("type")]
        public string Type;

        [JsonProperty ("word")]
        public string Word;
    }

    public partial class ThesaurusLexicalEntry
    {
        [JsonProperty ("language")]
        public string Language;

        [JsonProperty ("entries")]
        public ThesaurusEntry [] Entries;

        [JsonProperty ("derivativeOf")]
        public DictionaryDerivativeOf [] DerivativeOf;

        [JsonProperty ("grammaticalFeatures")]
        public DictionaryGrammaticalFeature [] GrammaticalFeatures;

        [JsonProperty ("notes")]
        public DictionaryCrossReference [] Notes;

        [JsonProperty ("text")]
        public string Text;

        [JsonProperty ("lexicalCategory")]
        public string LexicalCategory;

        [JsonProperty ("pronunciations")]
        public DictionaryPronunciation [] Pronunciations;

        [JsonProperty ("variantForms")]
        public DictionaryVariantForm [] VariantForms;
    }

    public partial class ThesaurusSense
    {
        [JsonProperty ("antonyms")]
        public DictionaryOnym [] Antonyms { get; set; }

        [JsonProperty ("synonyms")]
        public DictionaryOnym [] Synonyms { get; set; }

        [JsonProperty ("domains")]
        public string [] Domains;

        [JsonProperty ("pronunciations")]
        public DictionaryPronunciation [] Pronunciations;

        [JsonProperty ("crossReferences")]
        public DictionaryCrossReference [] CrossReferences;

        [JsonProperty ("crossReferenceMarkers")]
        public string [] CrossReferenceMarkers;

        [JsonProperty ("definitions")]
        public string [] Definitions;

        [JsonProperty ("id")]
        public string Id;

        [JsonProperty ("examples")]
        public DictionaryExample [] Examples;

        [JsonProperty ("notes")]
        public DictionaryCrossReference [] Notes;

        [JsonProperty ("registers")]
        public string [] Registers;

        [JsonProperty ("translations")]
        public DictionaryTranslation [] Translations;

        [JsonProperty ("regions")]
        public string [] Regions;

        [JsonProperty ("subsenses")]
        public ThesaurusSense [] Subsenses;

        [JsonProperty ("variantForms")]
        public DictionaryVariantForm [] VariantForms;
    }

    public partial class DictionaryOnym
    {
        [JsonProperty ("id")]
        public string Id { get; set; }

        [JsonProperty ("regions")]
        public string [] Regions { get; set; }

        [JsonProperty ("domains")]
        public string [] Domains { get; set; }

        [JsonProperty ("language")]
        public string Language { get; set; }

        [JsonProperty ("registers")]
        public string [] Registers { get; set; }

        [JsonProperty ("text")]
        public string Text { get; set; }
    }

    public partial class ThesaurusData
    {
        public static ThesaurusData FromJson (string json)
        {
            return JsonConvert.DeserializeObject<ThesaurusData> (json, Converter.Settings);
        }
    }

    public static partial class Serialize
    {
        public static string ToJson (this ThesaurusData self)
        {
            return JsonConvert.SerializeObject (self, Converter.Settings);
        }
    }
}