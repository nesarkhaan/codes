namespace FileValidate.Test;

public class MetaDataTestData {


    // test data for a merkle tree that is incomplete and has 16 chunks and 15 hashes
    public static string IncompleteOne_Identifier = "chae6ur5bookah8eihahPoojaef6Ieghoapohteesohw3ohj";
		
	public static string IncompleteOne_Path = "incomplete_1.tpk";


	public static string IncompleteOne_FileName = "incomplete_1.dat";

	public static int IncompleteOne_Size = 1048576;

	public static int IncompleteOne_NHashes = 15;

	public static int IncompleteOne_NChunks = 16;

	public static string[] IncompleteOne_Hashes = [
		"c19d2ab164312a999d7e495f214b3c6217a79ddbbd4b12402d620ca836a451dc",
		"62c7ce2369498e53c5d69318b7fcc2be3504564d414d9668ec2dcf76801605b5",
		"86ea5a9644c8b5a85cec531c8713df3c0544232df124da4f3577465c5b79df19",
		"1ce84b589d9485e554ddf3a109125dee3cd6c9e488d717ae301483f2fb08c8a1",
		"66bec9b34f15d7e6c45b53c138ade06571eb1a5d9c2e9e43ce6ae0a54f8b5a03",
		"379e7ec1199ccdedf501acc21280548449bcec39d19b41bd40584d8e68d778b9",
		"6d31f47dc69150e06cb2b721fa0e15b485e3b1be8014c07b8a33ddcb97987142",
		"62e245c814754d0a910208554af3dfa0660fdcf964a65bd757d620c0cc8644a5",
		"a37480d14f1e484b96e6bf4b5846a00d0734ab4fa5120e435b226a009180e468",
		"41db588337a1a443637cddfbc94a648d38279b3fc57e4efc19992e9ad67046dd",
		"ac8e2cfc1af209285a8f66154417cc0c29bf1a94bec45fcfb1154f002a6bdcf3",
		"d26bdcc9fc03f5526956caa7e11531b32fec83909bacd66d01b76825bcb425da",
		"de46066b6cccde530f0025719568b1357990791adbc33e5735479712600c9469",
		"072f5b70c3f4529da6452c297b24d5344e5156c0b88657c241198f62de5211ea",
		"745a3e79b89cae57128082d4a717faa9816a1e9ee3e925a38e4c401aa651ce84",
	];

	public static string[][] IncompleteOne_Chunks = new string[][] {
	 new string[] { "d13cc455996c45ea9532b4e5604eb211dcd7976f328c72ce84b9a4dcb23f9f5f","0","65536" },
	 new string[] { "39340a50780f586047cc5513a3dda67030650f54b9f8c57d48150457c0bf94f7","65536","65536" },
	 new string[] { "aef7f1d0cfbc6fa4fe7d0fbda609dd74dc1210db3d80f7fbec638207490cac4c","131072","65536" },
	 new string[] { "27808dd5d27d8516ff3894634a1c7dccd8dd04fd2e08e39a48f4ce8d0b1f9717","196608","65536" },
	 new string[] { "ac65cd467b2970e6b7c6bff82368b338e757b96849cb1fcd5162ca702b696f37","262144","65536" },
	 new string[] { "d4379784e5eebe2f617b276f1a09922bfafd696bbc65587b749cd4eda53851b2","327680","65536" },
	 new string[] { "5e2cea5efced21cd0337ca86012ac7c8d3dd0e30c132b725041867fe304fe34f","393216","65536" },
	 new string[] { "93748fe6a8ce994803fff2863898c80ad82edc69a9415c308c3173c0efd90a34","458752","65536" },
	 new string[] { "95bf59a0c390dcc85ba299ddf932ac75ffa573288197445cb396ce268be747b1","524288","65536" },
	 new string[] { "60a2db4d8f2950ac6e6c80ae51b1e8a21ad4ca954041745a8eefaaa0e7898104","589824","65536" },
	 new string[] { "865fadcd3966126f50559ec2e98a6bb471ad55ba5b1c97f7717a520cffaa0fd8","655360","65536" },
	 new string[] { "f4eec80b18d0100d19844e014661acd4c3a998e89a0498ce2f206541615798b8","720896","65536" },
	 new string[] { "82b9401be6008b0ca3905cf4d3041a35b9a952e44345b5f57a760942bdff400a","786432","65536" },
	 new string[] { "c22f238cd7857a715aa771a17f5c86730c2c6eb22559f38f007217100ad4d276","851968","65536" },
	 new string[] { "186122ec6988a51797b48395eb9fbc0293c05a7b341ae13e9193ffc95505221a","917504","65536" },
	 new string[] { "41773c1e7d18f86ea2dabf2baa21dd2195837513dbda5e113a91a06188eae9a7","983040","65536" },
	};

	// Module Incomplete 2

	public static string IncompleteTwo_Identifier = "oureiheLaneoshah5Iushuphahqu1ahxeeBae7vefahneibu";

	public static string IncompleteTwo_FileName = "incomplete_2.dat";

	public static int IncompleteTwo_Size = 1048576;

	public static int IncompleteTwo_NHashes = 15;

	public static int IncompleteTwo_NChunks = 16;

	public static string[] IncompleteTwo_Hashes = [
		"0e656a8be2b0c5e6fdd4bff2af8b253badc68bb20d982822403f43fec18e952a",
		"4dce7209361019a67a7dd8e298e3119730c908e217b3d325711a9dad7be92744",
		"ff1c4c3928990390f35e337300a71ba5d58daa62ea06d3f96b30521cc3a67e23",
		"2f9a74b4319441c3912206d8d1b6eaf67f4d4f1ab7905a9b8f0f40d58bc6cb70",
		"f0c85ed04a3fdf9db540ba55ff6e14988e2a08b960f2abba1326952feaa0307d",
		"5b62e0cb7f6647ce32a85fb11c2cfe94376bfc2b88ebc698013e0f0dcdf638a3",
		"cca1850d3cd9d45bbd2a2a8a8f4922d11400704bb6a45d8f6ef3a92a3e78afdf",
		"80be19eed653ff5abbedae7ee517349580b007a9a21dcdc65295a25f66db13e9",
		"0b96d278dd4efb514728099ba469fed552637252b3305f057b9a5d5c56bce6fe",
		"cf74312bb7ee8fc2fdc86abebcb8fda3d1eb62e10eebe8d5a341b2f6999f9456",
		"f3e42dd5ff9f460e0eea1b914b0851c1deb67bd11b228595317fb974691b3961",
		"ea4d63142b6779a705e996010960df9a113ff090c8454005fae0b757e5daadd8",
		"1467e0bc8f764ff985ba58ca64570194f71d8854cb65b29899433f0f0486ce49",
		"7a5d0aa540c7ca826c9401e36bf9569fae4a3dea9914df0aa3c3768973e260da",
		"7df3a1fd5730b347c9998adc015458a931029b52233d2cf35a75e7ee77a89da8",
	];

	public static string[][] IncompleteTwo_Chunks = new string[][] {
	 new string[] { "7d1af022ad996f2426fa79e78c9b4858b05f9a11d7f7565e3f6055d0272ab5a8","0","65536" },
	 new string[] { "9e76ee116184414c6d8069cd8d2d8d0a7031012693c138d2cae17c9d81936501","65536","65536" },
	 new string[] { "35929f64d97f12d73da569db3e12db580c96468b479279ef9d6c12f9b37cdff5","131072","65536" },
	 new string[] { "d23b54365d8eefa8f05d3a893f0f2d829b3ff30c539ba8fc4e1da16dc6c51e92","196608","65536" },
	 new string[] { "3a6b327b329bd652b6fd8768ecd657de353570c50965dbc6363535b98ae8f330","262144","65536" },
	 new string[] { "15d32d75ca9a40cceb83ae923ed922a944378af0f5f3d5bbeb66f96ec0d8f501","327680","65536" },
	 new string[] { "fa78b0b8000bf81b6dd7fdcc49976e5e798b0c626fbadec4ddd9325cf026388a","393216","65536" },
	 new string[] { "39d00ffdb485e6c3626c97aa080d91fbc0f6f505aad3e8f65bf818c26ed26e09","458752","65536" },
	 new string[] { "01f5cb318195c56f74b80b7ebe40ff82fab0ca8053e7c4f7653df76d0477ab14","524288","65536" },
	 new string[] { "da0daada7652610aad5bf26bf453a765d2e333454bfdc3d3c16d49157d0b54b0","589824","65536" },
	 new string[] { "8b818a632d772d7f2e25d51c3942333c7b89b7e5b642017df05c324c63141648","655360","65536" },
	 new string[] { "79ce5848270320a38c221a4e53dec5795f408d3962ba5d68b4ec0333855084b5","720896","65536" },
	 new string[] { "7440ea86c6ca2392cd5f67226d88cf34dbb83438eb6a9e8b1a12de31abff47e6","786432","65536" },
	 new string[] { "a83c0adb437aaf21e1aff3b14a6a219b714f5db58b7fd2b8480de0d5ced90afb","851968","65536" },
	 new string[] { "af925670c5e52658f64e1de87492052fb8abf1f6cdb7dbbf86b4c45cf0701745","917504","65536" },
	 new string[] { "1eae172ceae254701a7af7dac4bdcd4b49142c324a45b6e6275981a9d08cb46d","983040","65536" },
	};

	// Module Incomplete 3

	public static string IncompleteThree_Identifier = "voo2ung0Phae9ocaithualeipee9ofuac9uyei0reuk5toos";

	public static string IncompleteThree_FileName = "incomplete_3.dat";

	public static int IncompleteThree_Size = 2097152;

	public static int IncompleteThree_NHashes = 15;

	public static int IncompleteThree_NChunks = 16;

	public static string[] IncompleteThree_Hashes = [
		"dfcc98ea8253399e0bd6ec538986b370adaa4e40e5c98055f27ba2a591e8f3e1",
		"f165958497b1dd7c8d35cdf93f44beec9bdfc6f304d7332ae4c840a46e1ea23e",
		"21cfb8ad83c8e564e173c1df319db43ca7633dd35a03a84b6c9a46c19b043ac7",
		"3d294ba5446d81206b741d7589e6e29c072057e5d73af1090b411c726adbe731",
		"4730d73a1988678298609de4011c277b900094b6d344d888932eeeebcafe7ef0",
		"353c66b9ce36ae4c3cd0adc534dd2d478518018e9fad87f342958ea615ca129f",
		"52c08603f6cd3f0d65e3b2bfc86328830eac5bb62dd8b98a18ca77d011997263",
		"de20975f8401e5826eb30cde7eab117d6fb504bb4861137b5118fd42d77405c5",
		"259e2c41cec3a390e880df7e6a17c48d0ea318cf754cebaee4298d7886721a0d",
		"6c680fdd45cf6e19c1c6bb4221fbf259375d749ad9e02a95573c453ea29266d9",
		"22df745237ccb02a4e6ea7962e01926c327905816b17f1e6c6ca6d74a609ebb2",
		"5d9a4b4308a6df64078b0738dc06d856d95ae4ea71fca18cb044c94e68bea799",
		"024f3aafcfe68816e843df5150cb4bfac8f03ee220b31a7e9ef6226dd712685b",
		"c4a098e56e069be8f24ffdce35324914108f990e35c1fc2909a648fd57a677ec",
		"730d0e63e488dc631ec3653735ad23ea2a1d423796ac49b6a240e4a33bfec4d1",
	];

	public static string[][] IncompleteThree_Chunks = new string[][] {
	 new string[] { "4c86b6581be8cc1eb0ade20796eb38683b0ad17cfb3ec2f0fb915de2bb92f0bd","0","131072"},
	 new string[] { "b78f37bbb923b55a82ac61d3083ad526b4655589ecb7b7f9b75b29784ca0ebc0","131072","131072"},
	 new string[] { "33ecd12b09e357cc2e0780490edd48c6b6ea6549c6cc2853a025ca94590f75cb","262144","131072"},
	 new string[] { "7080a247ca7a0cfdbaf8151e6d24ccc38bc5273c19222a2e841206c8e25f13c7","393216","131072"},
	 new string[] { "47c1efad02e66664ac98f4147f372c97365fe722e0f670ceb9b0b5b12015c4d1","524288","131072"},
	 new string[] { "495a582fbeddce188e2ced2a28133aaee738c3c11aaacbb900d3aa27f7a698f7","655360","131072"},
	 new string[] { "f94f90bfa61e3ba74589f50fd5fbe2217a712b9feb0ad13c9943eba69a0a6b7d","786432","131072"},
	 new string[] { "da39d5f37e6409cd2b5ae8265503fd58b307dcb870914c8a5fd3ae79841f5a2c","917504","131072"},
	 new string[] { "31a4c8e94ce26862fc1d3538c48334b51376c95205ec5d47b0b584d309d5e9bf","1048576","131072"},
	 new string[] { "63fc18b36e982a19b161b0e0ce070752837eb42c5fb5ad91a07541b4dc7cba72","1179648","131072"},
	 new string[] { "1eb9a0366431ec484117a35c74d4b48977cd63f80d578ce1b67687925cbcfd64","1310720","131072"},
	 new string[] { "c2bac3c633c640f8e351b699a8ae3fdd891272394ff8f15d34d6b9811a494a22","1441792","131072"},
	 new string[] { "55f66c3ac64a96a1cb0aa434f74accc58d940e6ac3fa11b7f473909645595b58","1572864","131072"},
	 new string[] { "97878a2c4da07ce10a85d8a1405052c9b32f97dfb3781c1736f1b9c74e02fd4a","1703936","131072"},
	 new string[] { "7e10188ed1f5c1fd195a1624ba705fd5a60ed8fba3fd55aea5fc164ffd5e91d8","1835008","131072"},
	 new string[] { "4cc19fed2c651b0a9439a249b4dda4d0444687538accaca68299f4a3aa241ff5","1966080","131072"},
	};


	// Complete 1 Module
	public static string CompleteOne_Path = "./complete_1.tpk";
	
	public static string CompleteOne_Identifier = "iequ4ahjeewo6Nosa1Itoloa2ahfae5oiZaev1eimuo4aiCh";

	public static string CompleteOne_FileName = "complete_1.dat";

	public static int CompleteOne_Size = 3145728;

	public static int CompleteOne_NHashes = 15;

	public static int CompleteOne_NChunks = 16;

	public static string[] CompleteOne_Hashes = [
		"676682626c22e2b970902d00e7cf0eb83721de02450d62c9d21c1b121c86b64f",
		"f7e7b9ab4ff1a6abc0ae05e09477c00d98593bfbf7877b8d2cb2ea0c74bc4a6f",
		"46288777cfc97b091469cc6f07c1eee8dacafee129e014e2e37b90ca1c9f438d",
		"9d8e04e7bbf4593940a5d72796b915f01b347c889fa559e83be5a1976f19f0bf",
		"2ceca1b09948af4ad2cbac5b032853e9525d97f898dc37980df99b15bde933f0",
		"0519d0a745d0e792bead86daf7d38e3a098f43dfa22f0f9c788ef4d99530a134",
		"94707540e763e89acc6d0281debe598a897393b4bc30b4992b4b4b4f8d5591f4",
		"63bdaf4b6998de28a77d2508be36a58133e85dbb4b01ad16ca926af9bf21eec9",
		"b39625193f73f014a6f62ac889c47065702448b03777145c22f92893696229f7",
		"a3787da85af3632ac0346efc427cffa3c878945b3d2290d4fa932056fb1468c4",
		"a40e3e5a46027d5d9fcbed96164737923e668c923314bca67d19f69f25a8b014",
		"c7bc7f852a7d0de7a398e673d21e82be8639af1433c8ed3cc04f6fbc56424b17",
		"2bf886f36033f8805133548c83ded547713e4d96dd8a458f3ed1047576595dde",
		"552bb3d31b6e9768554b3bdd0bcbbcbc561c765df15272abb879130ef8edce8a",
		"afa21335b0280bfa8f977cdfc78db1d778a33161f54899f613673e9e894f1d4f",
	];

	public static string[][] CompleteOne_Chunks = new string[][] {
	 new string[] { "b228ef1e354e085f44280e921d5700fce3e808a9681b81ba46d50d3988313573","0", "196608" },
	 new string[] { "fe29058df15c567cf32dc23825159837b431863ca8f0f2878f73c2bb2e2be11b","196608","196608" },
	 new string[] { "97aaab2caf12489b64e703468f8bcedd4514acaec3132ce568008dd6a80cf4a1","393216","196608" },
	 new string[] { "2eb439690cb27425456dc9d81e2fcae7f854a369f058574558abd5c223f38e55","589824","196608" },
	 new string[] { "6b5658d1857cb2fb1566c970ad76cbed31c3629fddf2a5c3d5a136560d84157b","786432","196608" },
	 new string[] { "0d120e3cb12ffb3a845bbe391e5f5cff5de4fda5e69535faab2473d4abb9fc3d","983040","196608" },
	 new string[] { "96ea6466dfe4e93d632e3db753bb88776bb60a6250ab57a3daad874e8c2c8b08","1179648","196608" },
	 new string[] { "439694e57d5976df21d5935718da16d1e62ae3d0d21dff08030a0f5957f36feb","1376256","196608" },
	 new string[] { "77a10c932d24761507019bac19fab9d6d2ca952b7d2c567b2455a33c399355cd","1572864","196608" },
	 new string[] { "b4485ab7e4b7cabdba157878cd1bd61228e9879e504ff4a7f8e9be4ed8ec8cd7","1769472","196608" },
	 new string[] { "254396e37fbbf05939f84909336d92f9a6ec75b18f3afa355053f597e28d8643","1966080","196608" },
	 new string[] { "d2b713dc4182b95e4966a27cf88253e9c431f00670f4a2f17f2ec1746fcdb8de","2162688","196608" },
	 new string[] { "aa64b2d41547c188756cec877fbce542b4285f831384aae40c22cf6bf3093ce1","2359296","196608" },
	 new string[] { "ae46f38939b597ae628e446d01b13de1a2afb229160d04aa43bf984213906eda","2555904","196608" },
	 new string[] { "7154da3b11159d9b5c49e63fe4304150ca031a1a94479b5aad3bdd4121e71761","2752512","196608" },
	 new string[] { "67d612a886602289f6f0df6aaf552fda6b30a25103bc100cd6df76f92b46aaa6","2949120","196608" },
	};
	
	// Complete 2 Module
	public static string CompleteTwo_Path = "./complete_2.tpk";

	public static string CompleteTwo_Identifier = "iu1doow0eeChie2viyo9ohho9xoet7un2iaz4ohkiz0soo5d";

	public static string CompleteTwo_FileName = "complete_2.dat";

	public static int CompleteTwo_Size = 2097152;

	public static int CompleteTwo_NHashes = 15;

	public static int CompleteTwo_NChunks = 16;

	public static string[] CompleteTwo_Hashes = [
		"a5a9f78aba7fdf90fd8c013e48aaefea0a637fdc791a0452d17685ced79cf67f",
		"fadb158a70e0637ca5ef3f7a2ad2d0a6c90f4313ded3d3c413c2ae6c18c9d388",
		"3b4d8f9af580c7d75c364d3dc77d27a21e1cb1db82195d5e7d7a9bd0077b807c",
		"368b0b21ffafa5c69525449e5017ff29433c0cdfa549fac0020845b83e18651a",
		"4b78a15416f9b4c64c6b13a7bdb65395fd5611b6a8b29e5e6248a5715ece811a",
		"b1a4fb8bb65b62a7befc8bdcf96e122bb506e4e2ddf32a2a4b22fc25a8154de5",
		"7a21017eaa845b65037e0ffb9576e0c58ab6669f7e866ec1e8b983218efa9878",
		"f262477efbea65ccbaba2e35e0b58e6228b0f28151e360d415ccd0f8eb079651",
		"6e9cb62c8e17f70523d124975d6e3eb57920a7593b3d62e4ea794fce35fb5ebe",
		"da7475c028b03aef3bafd15dffdde528dfb03bd2aad8e616e3b12411bc97bea5",
		"01da8d3822461185a2bedcf01e931806f03b43ee7a2d81ba73ab4d88bbea0f09",
		"e233d342694296b6968de2f3b67debacdfa7aff072f34dad9852ee39300e0a11",
		"31d0802f5643d6164abf4721908d2aebe032bd33164fd4146cf71fe0c8dd592c",
		"8214f583126f07bf221b6b6fecb1ce3b19d2f510d6839c853f39a436f3e9fd43",
		"4633bd168d64c01351530b66a1d45fbbaa0fca583d499291cfc22c77b15cb6b4",
	];

	public static string[][] CompleteTwo_Chunks = new string[][] {
		new string[] { "58b7dac01559d3c40231478931d0f378d6641d046605a7704a3fcbfd0cca6c55","0","131072" },
		new string[] { "9f0e78c7fe5aebda557346883d4b6588c46929c1a025e7a80e715182fc0919bf","131072","131072" },
		new string[] { "3b12bdbc9c5fe622a9ff6e41893ac438d36b0232e2eb6899bd8fe1312f2d703c","262144","131072" },
		new string[] { "d612537ee5767ac55f7f4b1f11e0cf496792f6edfbfe27a236155d3692e40f07","393216","131072" },
		new string[] { "6545c4040e3c77af4d04d16e6679e3eadef0db5a092dbbfed798127f6937021d","524288","131072" },
		new string[] { "ec730d27e7803a030a9bdac8fa3776edafab6954ca7e697618298a5eceb26ef1","655360","131072" },
		new string[] { "0c3b56343b2115d8f5cc073169fe8ef29ba53c1c8de0dcb36808a11a2d6eb244","786432","131072" },
		new string[] { "05622285444de1e5eb8ff544de53e8ab775d603863de4ef9f056043af4508b25","917504","131072" },
		new string[] { "6108aecc4268ff61ba47c1d5b0fe1c4259d0057ee2b3fc585dbc94cf63356622","1048576","131072" },
		new string[] { "50a94f2d5ce7d493ad35ba4c935e3e02be6a3396663225c2fe648fdf966d8d1f","1179648","131072" },
		new string[] { "9e93ef0ef1642fe1cea88bcf1e492c016ad9ca86ae7320cfc8c9c329db1ae716","1310720","131072" },
		new string[] { "64a72ad5e211476bb0108fd11a20e697cf87841cd000b07894aece4029a6280f","1441792","131072" },
		new string[] { "5a936ba8baf521c9647cdaa372f9b959e60ab76d79a5fd91d0755b49fd431880","1572864","131072" },
		new string[] { "c339251909cba1214d25da384b79820794debbf930dbc0db536f18753dee1f40","1703936","131072" },
		new string[] { "0b7aab9ed70af7028bb121809508765ef4793f2bc8fea1230a71d019b707d378","1835008","131072" },
		new string[] { "67febd45342be2a5b972f5c90a9a639fd076f186b180f584d496fa8e55725cb2","1966080","131072" },
	};
	
	// Complete 3 Module
	public static string CompleteThree_Path = "./complete_3.tpk";

	public static string CompleteThree_Identifier = "KooChahsitiux1yug6Wiqu5phee0Eeth6eiGhaimo8caip1i";

	public static string CompleteThree_FileName = "complete_3.dat";

	public static int CompleteThree_Size = 2097152;

	public static int CompleteThree_NHashes = 15;

	public static int CompleteThree_NChunks = 16;

	public static string[] CompleteThree_Hashes = [
		"3778dcb21f5f1c80f66589c85b86a30d40e19ee30b5d64f52224bf05bfe83131",
		"b85ca65c397bf10cbbe9080e1f9aabdd5abd6cfe214b0e765f71bdab1eff27e5",
		"7b21f9fc5860627b22bef3f22efdd0d4c90902f5d0c3b67b698ddf6c9d30acc5",
		"4b2700418eb7aec716177836ecd9aff650f9404028256ccd210cdd4f22c523e1",
		"023d8ea77c2233a4006d7258f2520d554ecebbda23fa8a022fd692170d80ff48",
		"38ef209d506ce977eabde4b64d62f5d82870bcd29ec42a50edc61866b1274f61",
		"3829e7aa130f667458ef606fdda50984887a6dc41b025a0f638cf5b9e29cec6a",
		"8f70f291ad7de11fd4f0dce8ec428b4a8061d4085416b000055ad20a908a9412",
		"89fc38dae7cc8aa3664eccb01ef89748d557e76aa15beeaf25f778ccb88d383e",
		"59abe696f8819a78d87d293c691f610955fe492cda20d679e940c84603f58565",
		"f5b61095a2b7e4f4899e4a2c79ce2b7030d358d0c4e85d3e5c3377f716a2d995",
		"8d3467b7d8d59342632fbe8fc787fafdbd23584bc7c186cd68717ec8b3114d7b",
		"a7700257c10b03881d73dd6ff19c669d397ce98f00ae99ad771aa9cb8241ea32",
		"5cce4d24a9e48e5134ad219b1ceeef5b74b53e1d978e0aa56253c3ceef355a76",
		"6d1f3e798ca12464aab678e1962065f65698b9336a4954a63cf21bf8fc3d3bfb",
	];

	public static string[][] CompleteThree_Chunks = new string[][] {
	 new string[] { "58b7dac01559d3c40231478931d0f378d6641d046605a7704a3fcbfd0cca6c55","0","131072" } ,
	 new string[] { "9f0e78c7fe5aebda557346883d4b6588c46929c1a025e7a80e715182fc0919bf","131072","131072" } ,
	 new string[] { "3b12bdbc9c5fe622a9ff6e41893ac438d36b0232e2eb6899bd8fe1312f2d703c","262144","131072" } ,
	 new string[] { "d612537ee5767ac55f7f4b1f11e0cf496792f6edfbfe27a236155d3692e40f07","393216","131072" } ,
	 new string[] { "6545c4040e3c77af4d04d16e6679e3eadef0db5a092dbbfed798127f6937021d","524288","131072" } ,
	 new string[] { "ec730d27e7803a030a9bdac8fa3776edafab6954ca7e697618298a5eceb26ef1","655360","131072" } ,
	 new string[] { "0c3b56343b2115d8f5cc073169fe8ef29ba53c1c8de0dcb36808a11a2d6eb244","786432","131072" } ,
	 new string[] { "05622285444de1e5eb8ff544de53e8ab775d603863de4ef9f056043af4508b25","917504","131072" } ,
	 new string[] { "6108aecc4268ff61ba47c1d5b0fe1c4259d0057ee2b3fc585dbc94cf63356622","1048576","131072" } ,
	 new string[] { "50a94f2d5ce7d493ad35ba4c935e3e02be6a3396663225c2fe648fdf966d8d1f","1179648","131072" } ,
	 new string[] { "9e93ef0ef1642fe1cea88bcf1e492c016ad9ca86ae7320cfc8c9c329db1ae716","1310720","131072" } ,
	 new string[] { "64a72ad5e211476bb0108fd11a20e697cf87841cd000b07894aece4029a6280f","1441792","131072" } ,
	 new string[] { "5a936ba8baf521c9647cdaa372f9b959e60ab76d79a5fd91d0755b49fd431880","1572864","131072" } ,
	 new string[] { "c339251909cba1214d25da384b79820794debbf930dbc0db536f18753dee1f40","1703936","131072" } ,
	 new string[] { "0b7aab9ed70af7028bb121809508765ef4793f2bc8fea1230a71d019b707d378","1835008","131072" } ,
	 new string[] { "67febd45342be2a5b972f5c90a9a639fd076f186b180f584d496fa8e55725cb2","1966080","131072" } ,
	};
}
