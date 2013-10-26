namespace IdSharp.Tagging.ID3v2
{
    using System;
    using System.Collections.Generic;

    public static class LanguageHelper
    {
        static LanguageHelper()
        {
            LanguageHelper.m_SortedLanguages = new string[] { 
                "Abkhazian", "Achinese", "Acoli", "Adangme", "Afar", "Afrihili", "Afrikaans", "Afro-Asiatic (Other)", "Akan", "Akkadian", "Albanian", "Aleut", "Algonquian Languages", "Altaic (Other)", "Amharic", "Apache Languages", 
                "Arabic", "Aramaic", "Arapaho", "Araucanian", "Arawak", "Armenian", "Artificial (Other)", "Assamese", "Athapascan Languages", "Austronesian (Other)", "Avaric", "Avestan", "Awadhi", "Aymara", "Azerbaijani", "Aztec", 
                "Balinese", "Baltic (Other)", "Baluchi", "Bambara", "Bamileke Languages", "Banda", "Bantu (Other)", "Basa", "Bashkir", "Basque", "Beja", "Bemba", "Bengali", "Berber (Other)", "Bhojpuri", "Bihari", 
                "Bikol", "Bini", "Bislama", "Bosnian", "Braj", "Breton", "Buginese", "Bulgarian", "Buriat", "Burmese", "Byelorussian", "Caddo", "Carib", "Catalan", "Caucasian (Other)", "Cebuano", 
                "Celtic (Other)", "Central American Indian (Other)", "Chagatai", "Chamorro", "Chechen", "Cherokee", "Cheyenne", "Chibcha", "Chinese", "Chinook jargon", "Choctaw", "Church Slavic", "Chuvash", "Coptic", "Cornish", "Corsican", 
                "Cree", "Creek", "Creoles and Pidgins (Other)", "Creoles and Pidgins, English-based (Other)", "Creoles and Pidgins, French-based (Other)", "Creoles and Pidgins, Portuguese-based (Other)", "Croatian", "Cushitic (Other)", "Czech", "Dakota", "Danish", "Delaware", "Dinka", "Divehi", "Dogri", "Dravidian (Other)", 
                "Duala", "Dutch", "Dutch, Middle (ca. 1050-1350)", "Dyula", "Dzongkha", "Efik", "Egyptian (Ancient)", "Ekajuk", "Elamite", "English", "English, Old (ca. 450-1100)", "Eskimo (Other)", "Esperanto", "Estonian", "Ewe", "Ewondo", 
                "Fang", "Fanti", "Faroese", "Fijian", "Finnish", "Finno-Ugrian (Other)", "Fon", "French", "French, Middle (ca. 1400-1600)", "French, Old (842- ca. 1400)", "Frisian", "Fulah", "Ga", "Gaelic (Scots)", "Gallegan", "Ganda", 
                "Gayo", "Geez", "Georgian", "German", "German, Middle High (ca. 1050-1500)", "German, Old High (ca. 750-1050)", "Germanic (Other)", "Gilbertese", "Gondi", "Gothic", "Grebo", "Greek", "Greek, Ancient (to 1453)", "Greenlandic", "Guarani", "Gujarati", 
                "Haida", "Hausa", "Hawaiian", "Hebrew", "Herero", "Hiligaynon", "Himachali", "Hindi", "Hiri Motu", "Hungarian", "Hupa", "Iban", "Icelandic", "Igbo", "Ijo", "Iloko", 
                "Indic (Other)", "Indo-European (Other) Interlingue", "Indonesian", "Interlingua (International Auxiliary language Association)", "Inuktitut", "Inupiak", "Iranian (Other)", "Irish", "Irish, Middle (900 - 1200)", "Irish, Old (to 900)", "Iroquoian uages", "Italian", "Japanese", "Javanese", "Judeo-Arabic", "Judeo-Persian", 
                "Kabyle", "Kachin", "Kamba", "Kannada", "Kanuri", "Kara-Kalpak", "Karen", "Kashmiri", "Kawi", "Kazakh", "Khasi", "Khmer", "Khoisan (Other)", "Khotanese", "Kikuyu", "Kinyarwanda", 
                "Kirghiz", "Komi", "Kongo", "Konkani", "Korean", "Kpelle", "Kru", "Kuanyama", "Kumyk", "Kurdish", "Kurukh", "Kusaie", "Kutenai", "Ladino", "Lahnda", "Lamba", 
                "Langue d'Oc (post 1500)", "Lao", "Latin", "Latvian", "Letzeburgesch", "Lezghian", "Lingala", "Lithuanian", "Lozi", "Luba-Katanga", "Luiseno", "Lunda", "Luo (Kenya and Tanzania)", "Macedonian", "Macedonian Makasar", "Madurese", 
                "Magahi", "Maithili", "Malagasy", "Malay", "Malayalam", "Maltese", "Mandingo", "Manipuri", "Manobo Languages", "Manx", "Maori", "Marathi", "Mari", "Marshall", "Marwari", "Masai", 
                "Mayan Languages", "Mende", "Micmac", "Middle English (ca. 1100-1500)", "Minangkabau", "Miscellaneous (Other)", "Mohawk", "Moldavian", "Mongo", "Mongolian", "Mon-Kmer (Other)", "Mossi", "Multiple Languages", "Munda Languages", "Nauru", "Navajo", 
                "Ndebele, North", "Ndebele, South", "Ndongo", "Nepali", "Newari", "Niger-Kordofanian (Other)", "Nilo-Saharan (Other)", "Niuean", "Norse, Old", "North American Indian (Other)", "Norwegian", "Norwegian (Nynorsk)", "Nubian Languages", "Nyamwezi", "Nyanja", "Nyankole", 
                "Nyoro", "Nzima", "Ojibwa", "Oriya", "Oromo", "Osage", "Ossetic", "Otomian Languages", "Pahlavi", "Palauan", "Pali", "Pampanga", "Pangasinan", "Panjabi", "Papiamento", "Papuan-Australian (Other)", 
                "Persian", "Persian, Old (ca 600 - 400 B.C.)", "Phoenician", "Polish", "Ponape", "Portuguese", "Prakrit uages", "Provencal, Old (to 1500)", "Pushto", "Quechua", "Rajasthani", "Rarotongan", "Rhaeto-Romance", "Romance (Other)", "Romanian", "Romany", 
                "Rundi", "Russian", "Salishan Languages", "Samaritan Aramaic", "Sami Languages", "Samoan", "Sandawe", "Sango", "Sanskrit", "Sardinian", "Scots", "Selkup", "Semitic (Other)", "Serbian", "Serer", "Shan", 
                "Shona", "Sidamo", "Siksika", "Sindhi", "Singhalese", "Sino-Tibetan (Other)", "Siouan Languages", "Siswant Swazi", "Slavic (Other)", "Slovak", "Slovenian", "Sogdian", "Somali", "Songhai", "Sorbian Languages", "Sotho, Northern", 
                "Sotho, Southern", "South American Indian (Other)", "Spanish", "Sudanese", "Sukuma", "Sumerian", "Susu", "Swahili", "Swedish", "Syriac", "Tagalog", "Tahitian", "Tajik", "Tamashek", "Tamil", "Tatar", 
                "Telugu", "Tereno", "Thai", "Tibetan", "Tigre", "Tigrinya", "Timne", "Tivi", "Tlingit", "Tokelau", "Tonga (Nyasa)", "Tonga (Tonga Islands)", "Truk", "Tsimshian", "Tsonga", "Tswana", 
                "Tumbuka", "Turkish", "Turkish, Ottoman (1500 - 1928)", "Turkmen", "Tuvinian", "Twi", "Ugaritic", "Uighur", "Ukrainian", "Umbundu", "Undetermined", "Urdu", "Uzbek", "Vai", "Venda", "Vietnamese", 
                "Volap\x00fck", "Votic", "Wakashan Languages", "Walamo", "Waray", "Washo", "Welsh", "Wolof", "Xhosa", "Yakut", "Yao", "Yap", "Yiddish", "Yoruba", "Zapotec", "Zenaga", 
                "Zhuang", "Zulu", "Zuni"
             };
            LanguageHelper.m_Languages = new Dictionary<string, string>();
            LanguageHelper.m_Languages.Add("aar", "Afar");
            LanguageHelper.m_Languages.Add("abk", "Abkhazian");
            LanguageHelper.m_Languages.Add("ace", "Achinese");
            LanguageHelper.m_Languages.Add("ach", "Acoli");
            LanguageHelper.m_Languages.Add("ada", "Adangme");
            LanguageHelper.m_Languages.Add("afa", "Afro-Asiatic (Other)");
            LanguageHelper.m_Languages.Add("afh", "Afrihili");
            LanguageHelper.m_Languages.Add("afr", "Afrikaans");
            LanguageHelper.m_Languages.Add("aka", "Akan");
            LanguageHelper.m_Languages.Add("akk", "Akkadian");
            LanguageHelper.m_Languages.Add("alb", "Albanian");
            LanguageHelper.m_Languages.Add("ale", "Aleut");
            LanguageHelper.m_Languages.Add("alg", "Algonquian Languages");
            LanguageHelper.m_Languages.Add("amh", "Amharic");
            LanguageHelper.m_Languages.Add("ang", "English, Old (ca. 450-1100)");
            LanguageHelper.m_Languages.Add("apa", "Apache Languages");
            LanguageHelper.m_Languages.Add("ara", "Arabic");
            LanguageHelper.m_Languages.Add("arc", "Aramaic");
            LanguageHelper.m_Languages.Add("arm", "Armenian");
            LanguageHelper.m_Languages.Add("arn", "Araucanian");
            LanguageHelper.m_Languages.Add("arp", "Arapaho");
            LanguageHelper.m_Languages.Add("art", "Artificial (Other)");
            LanguageHelper.m_Languages.Add("arw", "Arawak");
            LanguageHelper.m_Languages.Add("asm", "Assamese");
            LanguageHelper.m_Languages.Add("ath", "Athapascan Languages");
            LanguageHelper.m_Languages.Add("ava", "Avaric");
            LanguageHelper.m_Languages.Add("ave", "Avestan");
            LanguageHelper.m_Languages.Add("awa", "Awadhi");
            LanguageHelper.m_Languages.Add("aym", "Aymara");
            LanguageHelper.m_Languages.Add("aze", "Azerbaijani");
            LanguageHelper.m_Languages.Add("bad", "Banda");
            LanguageHelper.m_Languages.Add("bai", "Bamileke Languages");
            LanguageHelper.m_Languages.Add("bak", "Bashkir");
            LanguageHelper.m_Languages.Add("bal", "Baluchi");
            LanguageHelper.m_Languages.Add("bam", "Bambara");
            LanguageHelper.m_Languages.Add("ban", "Balinese");
            LanguageHelper.m_Languages.Add("baq", "Basque");
            LanguageHelper.m_Languages.Add("bas", "Basa");
            LanguageHelper.m_Languages.Add("bat", "Baltic (Other)");
            LanguageHelper.m_Languages.Add("bej", "Beja");
            LanguageHelper.m_Languages.Add("bel", "Byelorussian");
            LanguageHelper.m_Languages.Add("bem", "Bemba");
            LanguageHelper.m_Languages.Add("ben", "Bengali");
            LanguageHelper.m_Languages.Add("ber", "Berber (Other)");
            LanguageHelper.m_Languages.Add("bho", "Bhojpuri");
            LanguageHelper.m_Languages.Add("bih", "Bihari");
            LanguageHelper.m_Languages.Add("bik", "Bikol");
            LanguageHelper.m_Languages.Add("bin", "Bini");
            LanguageHelper.m_Languages.Add("bis", "Bislama");
            LanguageHelper.m_Languages.Add("bla", "Siksika");
            LanguageHelper.m_Languages.Add("bnt", "Bantu (Other)");
            LanguageHelper.m_Languages.Add("bod", "Tibetan");
            LanguageHelper.m_Languages.Add("bra", "Braj");
            LanguageHelper.m_Languages.Add("bre", "Breton");
            LanguageHelper.m_Languages.Add("bua", "Buriat");
            LanguageHelper.m_Languages.Add("bug", "Buginese");
            LanguageHelper.m_Languages.Add("bul", "Bulgarian");
            LanguageHelper.m_Languages.Add("bur", "Burmese");
            LanguageHelper.m_Languages.Add("cad", "Caddo");
            LanguageHelper.m_Languages.Add("cai", "Central American Indian (Other)");
            LanguageHelper.m_Languages.Add("car", "Carib");
            LanguageHelper.m_Languages.Add("cat", "Catalan");
            LanguageHelper.m_Languages.Add("cau", "Caucasian (Other)");
            LanguageHelper.m_Languages.Add("ceb", "Cebuano");
            LanguageHelper.m_Languages.Add("cel", "Celtic (Other)");
            LanguageHelper.m_Languages.Add("ces", "Czech");
            LanguageHelper.m_Languages.Add("cha", "Chamorro");
            LanguageHelper.m_Languages.Add("chb", "Chibcha");
            LanguageHelper.m_Languages.Add("che", "Chechen");
            LanguageHelper.m_Languages.Add("chg", "Chagatai");
            LanguageHelper.m_Languages.Add("chi", "Chinese");
            LanguageHelper.m_Languages.Add("chm", "Mari");
            LanguageHelper.m_Languages.Add("chn", "Chinook jargon");
            LanguageHelper.m_Languages.Add("cho", "Choctaw");
            LanguageHelper.m_Languages.Add("chr", "Cherokee");
            LanguageHelper.m_Languages.Add("chu", "Church Slavic");
            LanguageHelper.m_Languages.Add("chv", "Chuvash");
            LanguageHelper.m_Languages.Add("chy", "Cheyenne");
            LanguageHelper.m_Languages.Add("cop", "Coptic");
            LanguageHelper.m_Languages.Add("cor", "Cornish");
            LanguageHelper.m_Languages.Add("cos", "Corsican");
            LanguageHelper.m_Languages.Add("cpe", "Creoles and Pidgins, English-based (Other)");
            LanguageHelper.m_Languages.Add("cpf", "Creoles and Pidgins, French-based (Other)");
            LanguageHelper.m_Languages.Add("cpp", "Creoles and Pidgins, Portuguese-based (Other)");
            LanguageHelper.m_Languages.Add("cre", "Cree");
            LanguageHelper.m_Languages.Add("crp", "Creoles and Pidgins (Other)");
            LanguageHelper.m_Languages.Add("cus", "Cushitic (Other)");
            LanguageHelper.m_Languages.Add("cym", "Welsh");
            LanguageHelper.m_Languages.Add("cze", "Czech");
            LanguageHelper.m_Languages.Add("dak", "Dakota");
            LanguageHelper.m_Languages.Add("dan", "Danish");
            LanguageHelper.m_Languages.Add("del", "Delaware");
            LanguageHelper.m_Languages.Add("deu", "German");
            LanguageHelper.m_Languages.Add("din", "Dinka");
            LanguageHelper.m_Languages.Add("div", "Divehi");
            LanguageHelper.m_Languages.Add("doi", "Dogri");
            LanguageHelper.m_Languages.Add("dra", "Dravidian (Other)");
            LanguageHelper.m_Languages.Add("dua", "Duala");
            LanguageHelper.m_Languages.Add("dum", "Dutch, Middle (ca. 1050-1350)");
            LanguageHelper.m_Languages.Add("dut", "Dutch");
            LanguageHelper.m_Languages.Add("dyu", "Dyula");
            LanguageHelper.m_Languages.Add("dzo", "Dzongkha");
            LanguageHelper.m_Languages.Add("efi", "Efik");
            LanguageHelper.m_Languages.Add("egy", "Egyptian (Ancient)");
            LanguageHelper.m_Languages.Add("eka", "Ekajuk");
            LanguageHelper.m_Languages.Add("ell", "Greek");
            LanguageHelper.m_Languages.Add("elx", "Elamite");
            LanguageHelper.m_Languages.Add("eng", "English");
            LanguageHelper.m_Languages.Add("enm", "Middle English (ca. 1100-1500)");
            LanguageHelper.m_Languages.Add("epo", "Esperanto");
            LanguageHelper.m_Languages.Add("esk", "Eskimo (Other)");
            LanguageHelper.m_Languages.Add("esl", "Spanish");
            LanguageHelper.m_Languages.Add("est", "Estonian");
            LanguageHelper.m_Languages.Add("eus", "Basque");
            LanguageHelper.m_Languages.Add("ewe", "Ewe");
            LanguageHelper.m_Languages.Add("ewo", "Ewondo");
            LanguageHelper.m_Languages.Add("fan", "Fang");
            LanguageHelper.m_Languages.Add("fao", "Faroese");
            LanguageHelper.m_Languages.Add("fas", "Persian");
            LanguageHelper.m_Languages.Add("fat", "Fanti");
            LanguageHelper.m_Languages.Add("fij", "Fijian");
            LanguageHelper.m_Languages.Add("fin", "Finnish");
            LanguageHelper.m_Languages.Add("fiu", "Finno-Ugrian (Other)");
            LanguageHelper.m_Languages.Add("fon", "Fon");
            LanguageHelper.m_Languages.Add("fra", "French");
            LanguageHelper.m_Languages.Add("fre", "French");
            LanguageHelper.m_Languages.Add("frm", "French, Middle (ca. 1400-1600)");
            LanguageHelper.m_Languages.Add("fro", "French, Old (842- ca. 1400)");
            LanguageHelper.m_Languages.Add("fry", "Frisian");
            LanguageHelper.m_Languages.Add("ful", "Fulah");
            LanguageHelper.m_Languages.Add("gaa", "Ga");
            LanguageHelper.m_Languages.Add("gae", "Gaelic (Scots)");
            LanguageHelper.m_Languages.Add("gai", "Irish");
            LanguageHelper.m_Languages.Add("gay", "Gayo");
            LanguageHelper.m_Languages.Add("gdh", "Gaelic (Scots)");
            LanguageHelper.m_Languages.Add("gem", "Germanic (Other)");
            LanguageHelper.m_Languages.Add("geo", "Georgian");
            LanguageHelper.m_Languages.Add("ger", "German");
            LanguageHelper.m_Languages.Add("gez", "Geez");
            LanguageHelper.m_Languages.Add("gil", "Gilbertese");
            LanguageHelper.m_Languages.Add("glg", "Gallegan");
            LanguageHelper.m_Languages.Add("gmh", "German, Middle High (ca. 1050-1500)");
            LanguageHelper.m_Languages.Add("goh", "German, Old High (ca. 750-1050)");
            LanguageHelper.m_Languages.Add("gon", "Gondi");
            LanguageHelper.m_Languages.Add("got", "Gothic");
            LanguageHelper.m_Languages.Add("grb", "Grebo");
            LanguageHelper.m_Languages.Add("grc", "Greek, Ancient (to 1453)");
            LanguageHelper.m_Languages.Add("gre", "Greek");
            LanguageHelper.m_Languages.Add("grn", "Guarani");
            LanguageHelper.m_Languages.Add("guj", "Gujarati");
            LanguageHelper.m_Languages.Add("hai", "Haida");
            LanguageHelper.m_Languages.Add("hau", "Hausa");
            LanguageHelper.m_Languages.Add("haw", "Hawaiian");
            LanguageHelper.m_Languages.Add("heb", "Hebrew");
            LanguageHelper.m_Languages.Add("her", "Herero");
            LanguageHelper.m_Languages.Add("hil", "Hiligaynon");
            LanguageHelper.m_Languages.Add("him", "Himachali");
            LanguageHelper.m_Languages.Add("hin", "Hindi");
            LanguageHelper.m_Languages.Add("hmo", "Hiri Motu");
            LanguageHelper.m_Languages.Add("hun", "Hungarian");
            LanguageHelper.m_Languages.Add("hup", "Hupa");
            LanguageHelper.m_Languages.Add("hye", "Armenian");
            LanguageHelper.m_Languages.Add("iba", "Iban");
            LanguageHelper.m_Languages.Add("ibo", "Igbo");
            LanguageHelper.m_Languages.Add("ice", "Icelandic");
            LanguageHelper.m_Languages.Add("ijo", "Ijo");
            LanguageHelper.m_Languages.Add("iku", "Inuktitut");
            LanguageHelper.m_Languages.Add("ilo", "Iloko");
            LanguageHelper.m_Languages.Add("ina", "Interlingua (International Auxiliary language Association)");
            LanguageHelper.m_Languages.Add("inc", "Indic (Other)");
            LanguageHelper.m_Languages.Add("ind", "Indonesian");
            LanguageHelper.m_Languages.Add("ine", "Indo-European (Other) Interlingue");
            LanguageHelper.m_Languages.Add("ipk", "Inupiak");
            LanguageHelper.m_Languages.Add("ira", "Iranian (Other)");
            LanguageHelper.m_Languages.Add("iri", "Irish");
            LanguageHelper.m_Languages.Add("iro", "Iroquoian uages");
            LanguageHelper.m_Languages.Add("isl", "Icelandic");
            LanguageHelper.m_Languages.Add("ita", "Italian");
            LanguageHelper.m_Languages.Add("jav", "Javanese");
            LanguageHelper.m_Languages.Add("jaw", "Javanese");
            LanguageHelper.m_Languages.Add("jpn", "Japanese");
            LanguageHelper.m_Languages.Add("jpr", "Judeo-Persian");
            LanguageHelper.m_Languages.Add("jrb", "Judeo-Arabic");
            LanguageHelper.m_Languages.Add("kaa", "Kara-Kalpak");
            LanguageHelper.m_Languages.Add("kab", "Kabyle");
            LanguageHelper.m_Languages.Add("kac", "Kachin");
            LanguageHelper.m_Languages.Add("kal", "Greenlandic");
            LanguageHelper.m_Languages.Add("kam", "Kamba");
            LanguageHelper.m_Languages.Add("kan", "Kannada");
            LanguageHelper.m_Languages.Add("kar", "Karen");
            LanguageHelper.m_Languages.Add("kas", "Kashmiri");
            LanguageHelper.m_Languages.Add("kat", "Georgian");
            LanguageHelper.m_Languages.Add("kau", "Kanuri");
            LanguageHelper.m_Languages.Add("kaw", "Kawi");
            LanguageHelper.m_Languages.Add("kaz", "Kazakh");
            LanguageHelper.m_Languages.Add("kha", "Khasi");
            LanguageHelper.m_Languages.Add("khi", "Khoisan (Other)");
            LanguageHelper.m_Languages.Add("khm", "Khmer");
            LanguageHelper.m_Languages.Add("kho", "Khotanese");
            LanguageHelper.m_Languages.Add("kik", "Kikuyu");
            LanguageHelper.m_Languages.Add("kin", "Kinyarwanda");
            LanguageHelper.m_Languages.Add("kir", "Kirghiz");
            LanguageHelper.m_Languages.Add("kok", "Konkani");
            LanguageHelper.m_Languages.Add("kom", "Komi");
            LanguageHelper.m_Languages.Add("kon", "Kongo");
            LanguageHelper.m_Languages.Add("kor", "Korean");
            LanguageHelper.m_Languages.Add("kpe", "Kpelle");
            LanguageHelper.m_Languages.Add("kro", "Kru");
            LanguageHelper.m_Languages.Add("kru", "Kurukh");
            LanguageHelper.m_Languages.Add("kua", "Kuanyama");
            LanguageHelper.m_Languages.Add("kum", "Kumyk");
            LanguageHelper.m_Languages.Add("kur", "Kurdish");
            LanguageHelper.m_Languages.Add("kus", "Kusaie");
            LanguageHelper.m_Languages.Add("kut", "Kutenai");
            LanguageHelper.m_Languages.Add("lad", "Ladino");
            LanguageHelper.m_Languages.Add("lah", "Lahnda");
            LanguageHelper.m_Languages.Add("lam", "Lamba");
            LanguageHelper.m_Languages.Add("lao", "Lao");
            LanguageHelper.m_Languages.Add("lat", "Latin");
            LanguageHelper.m_Languages.Add("lav", "Latvian");
            LanguageHelper.m_Languages.Add("lez", "Lezghian");
            LanguageHelper.m_Languages.Add("lin", "Lingala");
            LanguageHelper.m_Languages.Add("lit", "Lithuanian");
            LanguageHelper.m_Languages.Add("lol", "Mongo");
            LanguageHelper.m_Languages.Add("loz", "Lozi");
            LanguageHelper.m_Languages.Add("ltz", "Letzeburgesch");
            LanguageHelper.m_Languages.Add("lub", "Luba-Katanga");
            LanguageHelper.m_Languages.Add("lug", "Ganda");
            LanguageHelper.m_Languages.Add("lui", "Luiseno");
            LanguageHelper.m_Languages.Add("lun", "Lunda");
            LanguageHelper.m_Languages.Add("luo", "Luo (Kenya and Tanzania)");
            LanguageHelper.m_Languages.Add("mac", "Macedonian");
            LanguageHelper.m_Languages.Add("mad", "Madurese");
            LanguageHelper.m_Languages.Add("mag", "Magahi");
            LanguageHelper.m_Languages.Add("mah", "Marshall");
            LanguageHelper.m_Languages.Add("mai", "Maithili");
            LanguageHelper.m_Languages.Add("mak", "Macedonian Makasar");
            LanguageHelper.m_Languages.Add("mal", "Malayalam");
            LanguageHelper.m_Languages.Add("man", "Mandingo");
            LanguageHelper.m_Languages.Add("mao", "Maori");
            LanguageHelper.m_Languages.Add("map", "Austronesian (Other)");
            LanguageHelper.m_Languages.Add("mar", "Marathi");
            LanguageHelper.m_Languages.Add("mas", "Masai");
            LanguageHelper.m_Languages.Add("max", "Manx");
            LanguageHelper.m_Languages.Add("may", "Malay");
            LanguageHelper.m_Languages.Add("men", "Mende");
            LanguageHelper.m_Languages.Add("mga", "Irish, Middle (900 - 1200)");
            LanguageHelper.m_Languages.Add("mic", "Micmac");
            LanguageHelper.m_Languages.Add("min", "Minangkabau");
            LanguageHelper.m_Languages.Add("mis", "Miscellaneous (Other)");
            LanguageHelper.m_Languages.Add("mkh", "Mon-Kmer (Other)");
            LanguageHelper.m_Languages.Add("mlg", "Malagasy");
            LanguageHelper.m_Languages.Add("mlt", "Maltese");
            LanguageHelper.m_Languages.Add("mni", "Manipuri");
            LanguageHelper.m_Languages.Add("mno", "Manobo Languages");
            LanguageHelper.m_Languages.Add("moh", "Mohawk");
            LanguageHelper.m_Languages.Add("mol", "Moldavian");
            LanguageHelper.m_Languages.Add("mon", "Mongolian");
            LanguageHelper.m_Languages.Add("mos", "Mossi");
            LanguageHelper.m_Languages.Add("mri", "Maori");
            LanguageHelper.m_Languages.Add("msa", "Malay");
            LanguageHelper.m_Languages.Add("mul", "Multiple Languages");
            LanguageHelper.m_Languages.Add("mun", "Munda Languages");
            LanguageHelper.m_Languages.Add("mus", "Creek");
            LanguageHelper.m_Languages.Add("mwr", "Marwari");
            LanguageHelper.m_Languages.Add("mya", "Burmese");
            LanguageHelper.m_Languages.Add("myn", "Mayan Languages");
            LanguageHelper.m_Languages.Add("nah", "Aztec");
            LanguageHelper.m_Languages.Add("nai", "North American Indian (Other)");
            LanguageHelper.m_Languages.Add("nau", "Nauru");
            LanguageHelper.m_Languages.Add("nav", "Navajo");
            LanguageHelper.m_Languages.Add("nbl", "Ndebele, South");
            LanguageHelper.m_Languages.Add("nde", "Ndebele, North");
            LanguageHelper.m_Languages.Add("ndo", "Ndongo");
            LanguageHelper.m_Languages.Add("nep", "Nepali");
            LanguageHelper.m_Languages.Add("new", "Newari");
            LanguageHelper.m_Languages.Add("nic", "Niger-Kordofanian (Other)");
            LanguageHelper.m_Languages.Add("niu", "Niuean");
            LanguageHelper.m_Languages.Add("nla", "Dutch");
            LanguageHelper.m_Languages.Add("nno", "Norwegian (Nynorsk)");
            LanguageHelper.m_Languages.Add("non", "Norse, Old");
            LanguageHelper.m_Languages.Add("nor", "Norwegian");
            LanguageHelper.m_Languages.Add("nso", "Sotho, Northern");
            LanguageHelper.m_Languages.Add("nub", "Nubian Languages");
            LanguageHelper.m_Languages.Add("nya", "Nyanja");
            LanguageHelper.m_Languages.Add("nym", "Nyamwezi");
            LanguageHelper.m_Languages.Add("nyn", "Nyankole");
            LanguageHelper.m_Languages.Add("nyo", "Nyoro");
            LanguageHelper.m_Languages.Add("nzi", "Nzima");
            LanguageHelper.m_Languages.Add("oci", "Langue d'Oc (post 1500)");
            LanguageHelper.m_Languages.Add("oji", "Ojibwa");
            LanguageHelper.m_Languages.Add("ori", "Oriya");
            LanguageHelper.m_Languages.Add("orm", "Oromo");
            LanguageHelper.m_Languages.Add("osa", "Osage");
            LanguageHelper.m_Languages.Add("oss", "Ossetic");
            LanguageHelper.m_Languages.Add("ota", "Turkish, Ottoman (1500 - 1928)");
            LanguageHelper.m_Languages.Add("oto", "Otomian Languages");
            LanguageHelper.m_Languages.Add("paa", "Papuan-Australian (Other)");
            LanguageHelper.m_Languages.Add("pag", "Pangasinan");
            LanguageHelper.m_Languages.Add("pal", "Pahlavi");
            LanguageHelper.m_Languages.Add("pam", "Pampanga");
            LanguageHelper.m_Languages.Add("pan", "Panjabi");
            LanguageHelper.m_Languages.Add("pap", "Papiamento");
            LanguageHelper.m_Languages.Add("pau", "Palauan");
            LanguageHelper.m_Languages.Add("peo", "Persian, Old (ca 600 - 400 B.C.)");
            LanguageHelper.m_Languages.Add("per", "Persian");
            LanguageHelper.m_Languages.Add("phn", "Phoenician");
            LanguageHelper.m_Languages.Add("pli", "Pali");
            LanguageHelper.m_Languages.Add("pol", "Polish");
            LanguageHelper.m_Languages.Add("pon", "Ponape");
            LanguageHelper.m_Languages.Add("por", "Portuguese");
            LanguageHelper.m_Languages.Add("pra", "Prakrit uages");
            LanguageHelper.m_Languages.Add("pro", "Provencal, Old (to 1500)");
            LanguageHelper.m_Languages.Add("pus", "Pushto");
            LanguageHelper.m_Languages.Add("que", "Quechua");
            LanguageHelper.m_Languages.Add("raj", "Rajasthani");
            LanguageHelper.m_Languages.Add("rar", "Rarotongan");
            LanguageHelper.m_Languages.Add("roa", "Romance (Other)");
            LanguageHelper.m_Languages.Add("roh", "Rhaeto-Romance");
            LanguageHelper.m_Languages.Add("rom", "Romany");
            LanguageHelper.m_Languages.Add("ron", "Romanian");
            LanguageHelper.m_Languages.Add("rum", "Romanian");
            LanguageHelper.m_Languages.Add("run", "Rundi");
            LanguageHelper.m_Languages.Add("rus", "Russian");
            LanguageHelper.m_Languages.Add("sad", "Sandawe");
            LanguageHelper.m_Languages.Add("sag", "Sango");
            LanguageHelper.m_Languages.Add("sah", "Yakut");
            LanguageHelper.m_Languages.Add("sai", "South American Indian (Other)");
            LanguageHelper.m_Languages.Add("sal", "Salishan Languages");
            LanguageHelper.m_Languages.Add("sam", "Samaritan Aramaic");
            LanguageHelper.m_Languages.Add("san", "Sanskrit");
            LanguageHelper.m_Languages.Add("sco", "Scots");
            LanguageHelper.m_Languages.Add("scr", "Croatian");
            LanguageHelper.m_Languages.Add("sel", "Selkup");
            LanguageHelper.m_Languages.Add("sem", "Semitic (Other)");
            LanguageHelper.m_Languages.Add("sga", "Irish, Old (to 900)");
            LanguageHelper.m_Languages.Add("shn", "Shan");
            LanguageHelper.m_Languages.Add("sid", "Sidamo");
            LanguageHelper.m_Languages.Add("sin", "Singhalese");
            LanguageHelper.m_Languages.Add("sio", "Siouan Languages");
            LanguageHelper.m_Languages.Add("sit", "Sino-Tibetan (Other)");
            LanguageHelper.m_Languages.Add("sla", "Slavic (Other)");
            LanguageHelper.m_Languages.Add("slk", "Slovak");
            LanguageHelper.m_Languages.Add("slo", "Slovak");
            LanguageHelper.m_Languages.Add("slv", "Slovenian");
            LanguageHelper.m_Languages.Add("smi", "Sami Languages");
            LanguageHelper.m_Languages.Add("smo", "Samoan");
            LanguageHelper.m_Languages.Add("sna", "Shona");
            LanguageHelper.m_Languages.Add("snd", "Sindhi");
            LanguageHelper.m_Languages.Add("sog", "Sogdian");
            LanguageHelper.m_Languages.Add("som", "Somali");
            LanguageHelper.m_Languages.Add("son", "Songhai");
            LanguageHelper.m_Languages.Add("sot", "Sotho, Southern");
            LanguageHelper.m_Languages.Add("spa", "Spanish");
            LanguageHelper.m_Languages.Add("sqi", "Albanian");
            LanguageHelper.m_Languages.Add("srd", "Sardinian");
            LanguageHelper.m_Languages.Add("srr", "Serer");
            LanguageHelper.m_Languages.Add("ssa", "Nilo-Saharan (Other)");
            LanguageHelper.m_Languages.Add("ssw", "Siswant Swazi");
            LanguageHelper.m_Languages.Add("suk", "Sukuma");
            LanguageHelper.m_Languages.Add("sun", "Sudanese");
            LanguageHelper.m_Languages.Add("sus", "Susu");
            LanguageHelper.m_Languages.Add("sux", "Sumerian");
            LanguageHelper.m_Languages.Add("sve", "Swedish");
            LanguageHelper.m_Languages.Add("swa", "Swahili");
            LanguageHelper.m_Languages.Add("swe", "Swedish");
            LanguageHelper.m_Languages.Add("syr", "Syriac");
            LanguageHelper.m_Languages.Add("tah", "Tahitian");
            LanguageHelper.m_Languages.Add("tam", "Tamil");
            LanguageHelper.m_Languages.Add("tat", "Tatar");
            LanguageHelper.m_Languages.Add("tel", "Telugu");
            LanguageHelper.m_Languages.Add("tem", "Timne");
            LanguageHelper.m_Languages.Add("ter", "Tereno");
            LanguageHelper.m_Languages.Add("tgk", "Tajik");
            LanguageHelper.m_Languages.Add("tgl", "Tagalog");
            LanguageHelper.m_Languages.Add("tha", "Thai");
            LanguageHelper.m_Languages.Add("tib", "Tibetan");
            LanguageHelper.m_Languages.Add("tig", "Tigre");
            LanguageHelper.m_Languages.Add("tir", "Tigrinya");
            LanguageHelper.m_Languages.Add("tiv", "Tivi");
            LanguageHelper.m_Languages.Add("tli", "Tlingit");
            LanguageHelper.m_Languages.Add("tmh", "Tamashek");
            LanguageHelper.m_Languages.Add("tog", "Tonga (Nyasa)");
            LanguageHelper.m_Languages.Add("ton", "Tonga (Tonga Islands)");
            LanguageHelper.m_Languages.Add("tru", "Truk");
            LanguageHelper.m_Languages.Add("tsi", "Tsimshian");
            LanguageHelper.m_Languages.Add("tsn", "Tswana");
            LanguageHelper.m_Languages.Add("tso", "Tsonga");
            LanguageHelper.m_Languages.Add("tuk", "Turkmen");
            LanguageHelper.m_Languages.Add("tum", "Tumbuka");
            LanguageHelper.m_Languages.Add("tur", "Turkish");
            LanguageHelper.m_Languages.Add("tut", "Altaic (Other)");
            LanguageHelper.m_Languages.Add("twi", "Twi");
            LanguageHelper.m_Languages.Add("tyv", "Tuvinian");
            LanguageHelper.m_Languages.Add("uga", "Ugaritic");
            LanguageHelper.m_Languages.Add("uig", "Uighur");
            LanguageHelper.m_Languages.Add("ukr", "Ukrainian");
            LanguageHelper.m_Languages.Add("umb", "Umbundu");
            LanguageHelper.m_Languages.Add("und", "Undetermined");
            LanguageHelper.m_Languages.Add("urd", "Urdu");
            LanguageHelper.m_Languages.Add("uzb", "Uzbek");
            LanguageHelper.m_Languages.Add("vai", "Vai");
            LanguageHelper.m_Languages.Add("ven", "Venda");
            LanguageHelper.m_Languages.Add("vie", "Vietnamese");
            LanguageHelper.m_Languages.Add("vol", "Volap\x00fck");
            LanguageHelper.m_Languages.Add("vot", "Votic");
            LanguageHelper.m_Languages.Add("wak", "Wakashan Languages");
            LanguageHelper.m_Languages.Add("wal", "Walamo");
            LanguageHelper.m_Languages.Add("war", "Waray");
            LanguageHelper.m_Languages.Add("was", "Washo");
            LanguageHelper.m_Languages.Add("wel", "Welsh");
            LanguageHelper.m_Languages.Add("wen", "Sorbian Languages");
            LanguageHelper.m_Languages.Add("wol", "Wolof");
            LanguageHelper.m_Languages.Add("xho", "Xhosa");
            LanguageHelper.m_Languages.Add("yao", "Yao");
            LanguageHelper.m_Languages.Add("yap", "Yap");
            LanguageHelper.m_Languages.Add("yid", "Yiddish");
            LanguageHelper.m_Languages.Add("yor", "Yoruba");
            LanguageHelper.m_Languages.Add("zap", "Zapotec");
            LanguageHelper.m_Languages.Add("zen", "Zenaga");
            LanguageHelper.m_Languages.Add("zha", "Zhuang");
            LanguageHelper.m_Languages.Add("zho", "Chinese");
            LanguageHelper.m_Languages.Add("zul", "Zulu");
            LanguageHelper.m_Languages.Add("zun", "Zuni");
            LanguageHelper.m_Languages.Add("tkl", "Tokelau");
            LanguageHelper.m_Languages.Add("hrv", "Croatian");
            LanguageHelper.m_Languages.Add("bos", "Bosnian");
            LanguageHelper.m_Languages.Add("scc", "Serbian");
            LanguageHelper.m_Languages.Add("srp", "Serbian");
        }


        public static Dictionary<string, string> Languages
        {
            get
            {
                return LanguageHelper.m_Languages;
            }
        }

        public static string[] SortedLanguages
        {
            get
            {
                return LanguageHelper.m_SortedLanguages;
            }
        }


        private static readonly Dictionary<string, string> m_Languages;
        private static readonly string[] m_SortedLanguages;
    }
}

