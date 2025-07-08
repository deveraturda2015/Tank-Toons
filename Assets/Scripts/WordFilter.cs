using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class WordFilter
{
	private static List<string> FilteredWords;

	public static string ToFamilyFriendlyString(string input)
	{
		if (FilteredWords == null)
		{
			FillProfanityList();
		}
		foreach (string filteredWord in FilteredWords)
		{
			string replacement = new string('o', filteredWord.Length);
			input = Regex.Replace(input, filteredWord, replacement, RegexOptions.IgnoreCase);
		}
		if (input.Length > 20)
		{
			input = input.Substring(0, 20);
		}
		return input;
	}

	public static bool ContainsProfanity(string input)
	{
		if (FilteredWords == null)
		{
			FillProfanityList();
		}
		foreach (string filteredWord in FilteredWords)
		{
			string text = new string('o', filteredWord.Length);
			if (Regex.IsMatch(input, filteredWord, RegexOptions.IgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	private static void FillProfanityList()
	{
		FilteredWords = new List<string>();
		string text = "2g1c\n2 girls 1 cup\nacrotomophilia\nalabama hot pocket\nalaskan pipeline\nanal\nanilingus\nanus\napeshit\narsehole\nass\nasshole\nassmunch\nauto erotic\nautoerotic\nbabeland\nbaby batter\nbaby juice\nball gag\nball gravy\nball kicking\nball licking\nball sack\nball sucking\nbangbros\nbareback\nbarely legal\nbarenaked\nbastard\nbastardo\nbastinado\nbbw\nbdsm\nbeaner\nbeaners\nbeaver cleaver\nbeaver lips\nbestiality\nbig black\nbig breasts\nbig knockers\nbig tits\nbimbos\nbirdlock\nbitch\nbitches\nblack cock\nblonde action\nblonde on blonde action\nblowjob\nblow job\nblow your load\nblue waffle\nblumpkin\nbollocks\nbondage\nboner\nboob\nboobs\nbooty call\nbrown showers\nbrunette action\nbukkake\nbulldyke\nbullet vibe\nbullshit\nbung hole\nbunghole\nbusty\nbutt\nbuttcheeks\nbutthole\ncamel toe\ncamgirl\ncamslut\ncamwhore\ncarpet muncher\ncarpetmuncher\nchocolate rosebuds\ncirclejerk\ncleveland steamer\nclit\nclitoris\nclover clamps\nclusterfuck\ncock\ncocks\ncoprolagnia\ncoprophilia\ncornhole\ncoon\ncoons\ncreampie\ncum\ncumming\ncunnilingus\ncunt\ndarkie\ndate rape\ndaterape\ndeep throat\ndeepthroat\ndendrophilia\ndick\ndildo\ndingleberry\ndingleberries\ndirty pillows\ndirty sanchez\ndoggie style\ndoggiestyle\ndoggy style\ndoggystyle\ndog style\ndolcett\ndomination\ndominatrix\ndommes\ndonkey punch\ndouble dong\ndouble penetration\ndp action\ndry hump\ndvda\neat my ass\necchi\nejaculation\nerotic\nerotism\nescort\neunuch\nfaggot\nfecal\nfelch\nfellatio\nfeltch\nfemale squirting\nfemdom\nfigging\nfingerbang\nfingering\nfisting\nfoot fetish\nfootjob\nfrotting\nfuck\nfuck buttons\nfuckin\nfucking\nfucktards\nfudge packer\nfudgepacker\nfutanari\ngang bang\ngay sex\ngenitals\ngiant cock\ngirl on\ngirl on top\ngirls gone wild\ngoatcx\ngoatse\ngod damn\ngokkun\ngolden shower\ngoodpoop\ngoo girl\ngoregasm\ngrope\ngroup sex\ng-spot\nguro\nhand job\nhandjob\nhard core\nhardcore\nhentai\nhomoerotic\nhonkey\nhooker\nhot carl\nhot chick\nhow to kill\nhow to murder\nhuge fat\nhumping\nincest\nintercourse\njack off\njail bait\njailbait\njelly donut\njerk off\njigaboo\njiggaboo\njiggerboo\njizz\njuggs\nkike\nkinbaku\nkinkster\nkinky\nknobbing\nleather restraint\nleather straight jacket\nlemon party\nlolita\nlovemaking\nmake me come\nmale squirting\nmasturbate\nmenage a trois\nmilf\nmissionary position\nmotherfucker\nmound of venus\nmr hands\nmuff diver\nmuffdiving\nnambla\nnawashi\nnegro\nneonazi\nnigga\nnigger\nnig nog\nnimphomania\nnipple\nnipples\nnsfw images\nnude\nnudity\nnympho\nnymphomania\noctopussy\nomorashi\none cup two girls\none guy one jar\norgasm\norgy\npaedophile\npaki\npanties\npanty\npedobear\npedophile\npegging\npenis\nphone sex\npiece of shit\npissing\npiss pig\npisspig\nplayboy\npleasure chest\npole smoker\nponyplay\npoof\npoon\npoontang\npunany\npoop chute\npoopchute\nporn\nporno\npornography\nprince albert piercing\npthc\npubes\npussy\nqueaf\nqueef\nquim\nraghead\nraging boner\nrape\nraping\nrapist\nrectum\nreverse cowgirl\nrimjob\nrimming\nrosy palm\nrosy palm and her 5 sisters\nrusty trombone\nsadism\nsantorum\nscat\nschlong\nscissoring\nsemen\nsex\nsexo\nsexy\nshaved beaver\nshaved pussy\nshemale\nshibari\nshit\nshitblimp\nshitty\nshota\nshrimping\nskeet\nslanteye\nslut\ns&m\nsmut\nsnatch\nsnowballing\nsodomize\nsodomy\nspic\nsplooge\nsplooge moose\nspooge\nspread legs\nspunk\nstrap on\nstrapon\nstrappado\nstrip club\nstyle doggy\nsuck\nsucks\nsuicide girls\nsultry women\nswastika\nswinger\ntainted love\ntaste my\ntea bagging\nthreesome\nthroating\ntied up\ntight white\ntit\ntits\ntitties\ntitty\ntongue in a\ntopless\ntosser\ntowelhead\ntranny\ntribadism\ntub girl\ntubgirl\ntushy\ntwat\ntwink\ntwinkie\ntwo girls one cup\nundressing\nupskirt\nurethra play\nurophilia\nvagina\nvenus mound\nvibrator\nviolet wand\nvorarephilia\nvoyeur\nvulva\nwank\nwetback\nwet dream\nwhite power\nwrapping men\nwrinkled starfish\nxx\nxxx\nyaoi\nyellow showers\nyiffy\nzoophilia";
		string[] source = text.Split('\n');
		FilteredWords = source.ToList();
	}
}
