using Newtonsoft.Json;

namespace BotMyst.Data
{
    public partial class DictionaryData
    {
        [JsonProperty ("metadata")]
        public DictionaryMetadata Metadata;

        [JsonProperty ("results")]
        public DictionaryResult [] Results;
    }

    public partial class DictionaryMetadata
    {
    }

    public partial class DictionaryResult
    {
        [JsonProperty ("language")]
        public string Language;

        [JsonProperty ("pronunciations")]
        public DictionaryPronunciation [] Pronunciations;

        [JsonProperty ("id")]
        public string Id;

        [JsonProperty ("lexicalEntries")]
        public DictionaryLexicalEntry [] LexicalEntries;

        [JsonProperty ("type")]
        public string Type;

        [JsonProperty ("word")]
        public string Word;
    }

    public partial class DictionaryPronunciation
    {
        [JsonProperty ("dialects")]
        public string [] Dialects;

        [JsonProperty ("phoneticSpelling")]
        public string PhoneticSpelling;

        [JsonProperty ("audioFile")]
        public string AudioFile;

        [JsonProperty ("phoneticNotation")]
        public string PhoneticNotation;

        [JsonProperty ("regions")]
        public string [] Regions;
    }

    public partial class DictionaryLexicalEntry
    {
        [JsonProperty ("language")]
        public string Language;

        [JsonProperty ("entries")]
        public DictionaryEntry [] Entries;

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

    public partial class DictionaryEntry
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
        public DictionarySense [] Senses;

        [JsonProperty ("pronunciations")]
        public DictionaryPronunciation [] Pronunciations;

        [JsonProperty ("variantForms")]
        public DictionaryVariantForm [] VariantForms;
    }

    public partial class DictionarySense
    {
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
        public DictionarySense [] Subsenses;

        [JsonProperty ("variantForms")]
        public DictionaryVariantForm [] VariantForms;
    }

    public partial class DictionaryExample
    {
        [JsonProperty ("regions")]
        public string [] Regions;

        [JsonProperty ("domains")]
        public string [] Domains;

        [JsonProperty ("definitions")]
        public string [] Definitions;

        [JsonProperty ("notes")]
        public DictionaryCrossReference [] Notes;

        [JsonProperty ("senseIds")]
        public string [] SenseIds;

        [JsonProperty ("registers")]
        public string [] Registers;

        [JsonProperty ("text")]
        public string Text;

        [JsonProperty ("translations")]
        public DictionaryTranslation [] Translations;
    }

    public partial class DictionaryTranslation
    {
        [JsonProperty ("notes")]
        public DictionaryCrossReference [] Notes;

        [JsonProperty ("grammaticalFeatures")]
        public DictionaryGrammaticalFeature [] GrammaticalFeatures;

        [JsonProperty ("domains")]
        public string [] Domains;

        [JsonProperty ("language")]
        public string Language;

        [JsonProperty ("registers")]
        public string [] Registers;

        [JsonProperty ("regions")]
        public string [] Regions;

        [JsonProperty ("text")]
        public string Text;
    }

    public partial class DictionaryDerivativeOf
    {
        [JsonProperty ("id")]
        public string Id;

        [JsonProperty ("regions")]
        public string [] Regions;

        [JsonProperty ("domains")]
        public string [] Domains;

        [JsonProperty ("language")]
        public string Language;

        [JsonProperty ("registers")]
        public string [] Registers;

        [JsonProperty ("text")]
        public string Text;
    }

    public partial class DictionaryGrammaticalFeature
    {
        [JsonProperty ("text")]
        public string Text;

        [JsonProperty ("type")]
        public string Type;
    }

    public partial class DictionaryCrossReference
    {
        [JsonProperty ("text")]
        public string Text;

        [JsonProperty ("id")]
        public string Id;

        [JsonProperty ("type")]
        public string Type;
    }

    public partial class DictionaryVariantForm
    {
        [JsonProperty ("regions")]
        public string [] Regions;

        [JsonProperty ("text")]
        public string Text;
    }


    public partial class DictionaryData
    {
        public static DictionaryData FromJson (string json) => JsonConvert.DeserializeObject<DictionaryData> (json, Converter.Settings);
    }

    public static partial class Serialize
    {
        public static string ToJson (this DictionaryData self) => JsonConvert.SerializeObject (self, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}