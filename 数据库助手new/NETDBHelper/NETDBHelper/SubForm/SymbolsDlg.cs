using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NETDBHelper.SubForm
{
    public partial class SymbolsDlg : SubBaseDlg
    {
        public SymbolsDlg()
        {
            InitializeComponent();
        }

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			this.WBSymbols.DocumentText = @"<html xmlns=""http://www.w3.org/1999/xhtml""><head>

			  <style type=""text/css"">
*{
	padding:0px;
	margin:0px;
}
html{
	background:#FFF;
}
body{
	font:12px Verdana,Arial,Tahoma;
    width:780px;
}
#emoji1{
		font-family: ""Apple Color Emoji"",""Segoe UI Emoji"",NotoColorEmoji,""Segoe UI Symbol"",""Android Emoji"",EmojiSymbols;
}
                   .tbox dd{
	border-top:1px solid #C5DDBC;
}
.listbox th {
    height: 25px;
    line-height: 25px;
    text-align: center;
    background: #EFF6EB;
    font-weight: normal;
}
.listbox td {
    height: 40px;
    line-height: 40px;
    font-size: 20px;
    padding: 2px 5px;
    letter-spacing: 5px;
    word-break: break-all;
}
.tbox dd strong{
	color:#696;
}
.tbox dd h3{
	font-size:12px;
}
.tbox{
	border-bottom:1px dotted #E5EFD6;
}

.tbox table {
    width: 98%;
    border-collapse: collapse;
    margin-left: 8px;
}

.tbox table td{
    border-collapse: collapse;
    width: 100%;
    margin-left: 8px;
}
              </style>

</head><body>
<div class=""pleft"" style=""margin-top:2px;"">
    <div class=""listbox"">
      <dl class=""tbox ta"">
        <dd>
			<table>
        <tbody><tr><th><a href=""#teshufuhao"">特殊符号</a> <a href=""#bianhaoxuhao"">编号序号</a> <a href=""#shuxuefuhao"">数学符号</a> <a href=""#shangbiaoxiabiao"">上标下标</a> <a href=""#aixinfuhao"">爱心符号</a> <a href=""#biaodianfuhao"">标点符号</a> <a href=""#danweifuhao"">单位符号</a> <a href=""#huobifuhao"">货币符号</a> <a href=""#jiantoufuhao"">箭头符号</a> <a href=""#fuhaotuan"">符号图案</a> <a href=""#xilazimu"">希腊字母</a><br> <a href=""#hanyupinyin"">汉语拼音</a> <a href=""#zhongwenzifu"">中文字符</a> <a href=""#riyu"">日语字符</a> <a href=""#eyuzimu"">俄语字母</a> <a href=""#zhibiaofu"">制表符</a> <a href=""#huangguanfuhao"">皇冠符号</a> <a href=""emoji.html"">emoji</a></th></tr>
<tr><td>❤❥웃유♋☮✌☏☢☠✔☑♚▲♪✈✞÷↑↓◆◇⊙■□△▽¿─│♥❣♂♀☿Ⓐ✍✉☣☤✘☒♛▼♫⌘☪≈←→◈◎☉★☆⊿※¡━┃♡ღツ☼☁❅♒✎©®™Σ✪✯☭➳卐√↖↗●◐Θ◤◥︻〖〗┄┆℃℉°✿ϟ☃☂✄¢€£∞✫★½✡×↙↘○◑⊕◣◢︼【】┅┇☽☾✚〓▂▃▄▅▆▇█▉▊▋▌▍▎▏↔↕☽☾の•▸◂▴▾┈┊①②③④⑤⑥⑦⑧⑨⑩ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩ㍿▓♨♛❖♓☪✙┉┋☹☺☻تヅツッシÜϡﭢ™℠℗©®♥❤❥❣❦❧♡۵웃유ღ♋♂♀☿☼☀☁☂☄☾☽❄☃☈⊙☉℃℉❅✺ϟ☇♤♧♡♢♠♣♥♦☜☞☝✍☚☛☟✌✽✾✿❁❃❋❀⚘☑✓✔√☐☒✗✘ㄨ✕✖✖⋆✢✣✤✥❋✦✧✩✰✪✫✬✭✮✯❂✡★✱✲✳✴✵✶✷✸✹✺✻✼❄❅❆❇❈❉❊†☨✞✝☥☦☓☩☯☧☬☸✡♁✙♆。，、＇：∶；?‘’“”〝〞ˆˇ﹕︰﹔﹖﹑•¨….¸;！´？！～—ˉ｜‖＂〃｀@﹫¡¿﹏﹋﹌︴々﹟#﹩$﹠&amp;﹪%*﹡﹢﹦﹤‐￣¯―﹨ˆ˜﹍﹎+=&lt;＿_-\ˇ~﹉﹊（）〈〉‹›﹛﹜『』〖〗［］《》〔〕{}「」【】︵︷︿︹︽_﹁﹃︻︶︸﹀︺︾ˉ﹂﹄︼☩☨☦✞✛✜✝✙✠✚†‡◉○◌◍◎●◐◑◒◓◔◕◖◗❂☢⊗⊙◘◙◍⅟½⅓⅕⅙⅛⅔⅖⅚⅜¾⅗⅝⅞⅘≂≃≄≅≆≇≈≉≊≋≌≍≎≏≐≑≒≓≔≕≖≗≘≙≚≛≜≝≞≟≠≡≢≣≤≥≦≧≨≩⊰⊱⋛⋚∫∬∭∮∯∰∱∲∳%℅‰‱㊣㊎㊍㊌㊋㊏㊐㊊㊚㊛㊤㊥㊦㊧㊨㊒㊞㊑㊒㊓㊔㊕㊖㊗㊘㊜㊝㊟㊠㊡㊢㊩㊪㊫㊬㊭㊮㊯㊰㊙㉿囍♔♕♖♗♘♙♚♛♜♝♞♟ℂℍℕℙℚℝℤℬℰℯℱℊℋℎℐℒℓℳℴ℘ℛℭ℮ℌℑℜℨ♪♫♩♬♭♮♯°øⒶ☮✌☪✡☭✯卐✐✎✏✑✒✍✉✁✂✃✄✆✉☎☏➟➡➢➣➤➥➦➧➨➚➘➙➛➜➝➞➸♐➲➳⏎➴➵➶➷➸➹➺➻➼➽←↑→↓↔↕↖↗↘↙↚↛↜↝↞↟↠↡↢↣↤↥↦↧↨➫➬➩➪➭➮➯➱↩↪↫↬↭↮↯↰↱↲↳↴↵↶↷↸↹↺↻↼↽↾↿⇀⇁⇂⇃⇄⇅⇆⇇⇈⇉⇊⇋⇌⇍⇎⇏⇐⇑⇒⇓⇔⇕⇖⇗⇘⇙⇚⇛⇜⇝⇞⇟⇠⇡⇢⇣⇤⇥⇦⇧⇨⇩⇪➀➁➂➃➄➅➆➇➈➉➊➋➌➍➎➏➐➑➒➓㊀㊁㊂㊃㊄㊅㊆㊇㊈㊉ⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩ⒜⒝⒞⒟⒠⒡⒢⒣⒤⒥⒦⒧⒨⒩⒪⒫⒬⒭⒮⒯⒰⒱⒲⒳⒴⒵ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩⅪⅫⅬⅭⅮⅯⅰⅱⅲⅳⅴⅵⅶⅷⅸⅹⅺⅻⅼⅽⅾⅿ┌┍┎┏┐┑┒┓└┕┖┗┘┙┚┛├┝┞┟┠┡┢┣┤┥┦┧┨┩┪┫┬┭┮┯┰┱┲┳┴┵┶┷┸┹┺┻┼┽┾┿╀╁╂╃╄╅╆╇╈╉╊╋╌╍╎╏═║╒╓╔╕╖╗╘╙╚╛╜╝╞╟╠╡╢╣╤╥╦╧╨╩╪╫╬◤◥◄►▶◀◣◢▲▼◥▸◂▴▾△▽▷◁⊿▻◅▵▿▹◃❏❐❑❒▀▁▂▃▄▅▆▇▉▊▋█▌▍▎▏▐░▒▓▔▕■□▢▣▤▥▦▧▨▩▪▫▬▭▮▯㋀㋁㋂㋃㋄㋅㋆㋇㋈㋉㋊㋋㏠㏡㏢㏣㏤㏥㏦㏧㏨㏩㏪㏫㏬㏭㏮㏯㏰㏱㏲㏳㏴㏵㏶㏷㏸㏹㏺㏻㏼㏽㏾㍙㍚㍛㍜㍝㍞㍟㍠㍡㍢㍣㍤㍥㍦㍧㍨㍩㍪㍫㍬㍭㍮㍯㍰㍘☰☲☱☴☵☶☳☷☯</td></tr>
				<tr>
				</tr><tr><th id=""teshufuhao"">特殊符号<span id=""rtop""><a href=""#top"">↑返回顶部</a></span></th></tr>
                <tr><td>♠♣♧♡♥❤❥❣♂♀✲☀☼☾☽◐◑☺☻☎☏✿❀№↑↓←→√×÷★℃℉°◆◇⊙■□△▽¿½☯✡㍿卍卐♂♀✚〓㎡♪♫♩♬㊚㊛囍㊒㊖Φ♀♂‖$@*&amp;#※卍卐Ψ♫♬♭♩♪♯♮⌒¶∮‖€￡¥$</td></tr>
				<tr><th id=""bianhaoxuhao"">编号序号<span id=""rtop""><a href=""#top"">↑返回顶部</a></span></th></tr>
<tr><td>
①②③④⑤⑥⑦⑧⑨⑩⑪⑫⑬⑭⑮⑯⑰⑱⑲⑳⓪⓿❶❷❸❹❺❻❼❽❾❿⓫⓬⓭⓮⓯⓰⓱⓲⓳⓴⓵⓶⓷⓸⓹⓺⓻⓼⓽⓾㊀㊁㊂㊃㊄㊅㊆㊇㊈㊉㈠㈡㈢㈣㈤㈥㈦㈧㈨㈩⑴⑵⑶⑷⑸⑹⑺⑻⑼⑽⑾⑿⒀⒁⒂⒃⒄⒅⒆⒇⒈⒉⒊⒋⒌⒍⒎⒏⒐⒑⒒⒓⒔⒕⒖⒗⒘⒙⒚⒛ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩⅪⅫⅰⅱⅲⅳⅴⅵⅶⅷⅸⅹⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩ⒜⒝⒞⒟⒠⒡⒢⒣⒤⒥⒦⒧⒨⒩⒪⒫⒬⒭⒮⒯⒰⒱⒲⒳⒴⒵</td></tr> 
               <tr><th id=""shuxuefuhao"">数学符号<span id=""rtop""><a href=""#top"">↑返回顶部</a></span></th></tr>
<tr><td>
﹢﹣×÷±+-*/^=≌∽≦≧≒﹤﹥≈≡≠≤≥≮≯∷∶∝∞∧∨∑∏∪∩∈∵∴⊥∥∠⌒⊙√∛∜∟⊿㏒㏑%‰⅟½⅓⅕⅙⅐⅛⅑⅒⅔¾⅖⅗⅘⅚⅜⅝⅞≂≃≄≅≆≇≉≊≋≍≎≏≐≑≓≔≕≖≗≘≙≚≛≜≝≞≟≢≣≨≩⊰⊱⋛⋚∫∮∬∭∯∰∱∲∳℅øπ∀∁∂∃∄∅∆∇∉∊∋∌∍∎∐−∓∔∕∖∗∘∙∡∢∣∤∦∸∹∺∻∼∾∿≀≁≪≫≬≭≰≱≲≳≴≵≶≷≸≹≺≻≼≽≾≿⊀⊁⊂⊃⊄⊅⊆⊇⊈⊉⊊⊋⊌⊍⊎⊏⊐⊑⊒⊓⊔⊕⊖⊗⊘⊚⊛⊜⊝⊞⊟⊠⊡⊢⊣⊤⊦⊧⊨⊩⊪⊫⊬⊭⊮⊯⊲⊳⊴⊵⊶⊷⊸⊹⊺⊻⊼⊽⊾⋀⋁⋂⋃⋄⋅⋆⋇⋈⋉⋊⋋⋌⋍⋎⋏⋐⋑⋒⋓⋔⋕⋖⋗⋘⋙⋜⋝⋞⋟⋠⋡⋢⋣⋤⋥⋦⋧⋨⋩⋪⋫⋬⋭⋮⋯⋰⋱⋲⋳⋴⋵⋶⋷⋸⋹⋺⋻⋼⋽⋾⋿ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩⅪⅫⅬⅭⅮⅯↁↂↃↅↆↇↈ↉↊↋■□▢▣▤▥▦▧▨▩▪▫▬▭▮▯▰▱▲△▴▵▶▷▸▹►▻▼▽▾▿◀◁◂◃◄◅◆◇◈◉◊○◌◍◎●◐◑◒◓◔◕◖◗◘◙◚◛◜◝◞◟◠◡◢◣◤◥◦◧◨◩◪◫◬◭◮◯◰◱◲◳◴◵◶◷◸◹◺◿◻◼◽◾⏢⏥⌓⌔⌖
</td></tr>
				<tr><th id=""shangbiaoxiabiao"">上标下标<span id=""rtop""><a href=""#top"">↑返回顶部</a></span></th></tr>
				<tr><td>⁰ ¹ ² ³ ⁴ ⁵ ⁶ ⁷ ⁸ ⁹ ⁺ ⁻ ⁼ ⁽ ⁾ ⁿ ₀ ₁ ₂ ₃ ₄ ₅ ₆ ₇ ₈ ₉ ₊ ₋ ₌ ₍ ₎ ₐ ₑ ₒ ₓ ₔ ₕ ₖ ₗ ₘ ₙ ₚ ₛ ₜ</td></tr>
                <tr><th id=""aixinfuhao"">爱心符号<span id=""rtop""><a href=""#top"">↑返回顶部</a></span></th></tr>
		<tr><td>♥❣ღ♠♡♤❤❥</td></tr>
				<tr><th id=""biaodianfuhao"">标点符号<span id=""rtop""><a href=""#top"">↑返回顶部</a></span></th></tr>
				<tr><td>。，、＇：∶；?‘’“”〝〞ˆˇ﹕︰﹔﹖﹑•¨….¸;！´？！～—ˉ｜‖＂〃｀@﹫¡¿﹏﹋﹌︴々﹟#﹩$﹠&amp;﹪%*﹡﹢﹦﹤‐￣¯―﹨ˆ˜﹍﹎+=&lt;＿_-\ˇ~﹉﹊（）〈〉‹›﹛﹜『』〖〗［］《》〔〕{}「」【】︵︷︿︹︽_﹁﹃︻︶︸﹀︺︾ˉ﹂﹄︼❝❞‐‑‒–―‖‗‘’‚‛“”„‟†‡•‣․‥…‧&#x202A;&#x202B;&#x202C;&#x202D;&#x202E; ‰‱′″‴‵‶‷‸※‼‽‾‿⁀⁁⁂⁃⁄⁇⁈⁉⁊⁋⁌⁍⁎⁏⁐⁑⁒⁓⁔⁕⁖⁗⁘⁙⁚⁛⁜⁝⁞ &NoBreak;⁡⁢⁣⁤</td></tr>
				<tr><th id=""danweifuhao"">单位符号<span id=""rtop""><a href=""#top"">↑返回顶部</a></span></th></tr>
				<tr><td>°′″＄￥〒￠￡％＠℃℉﹩﹪‰﹫㎡㎥m²m³㎜㎟㎣㎝㎠㎤㍷㍸㍹㎞㎢㎦㏎㎚㎛㏕㎍㎎㎏㏄º○¤%$º¹²³㍺㎀㎁㎂㎃㎄㎅㎆㎇㎈㎉㎊㎋㎌㎐㎑㎒㎓㎔㎕㎖㎗㎘㎙㎧㎨㎩㎪㎫㎬㎭㎮㎯㎰㎱㎲㎳㎴㎵㎶㎷㎸㎹㎺㎻㎼㎽㎾㎿㏀㏁㏂㏃㏄㏅㏆㏇㏈㏉㏊㏋㏌㏍㏎㏏㏐㏑㏒㏓㏔㏕㏖㏗㏘㏙㏚㏛㏜㏝㏞㏟㍱㍲㍳㍴㍵㍶</td></tr>
				<tr><th id=""huobifuhao"">货币符号<span id=""rtop""><a href=""#top"">↑返回顶部</a></span></th></tr>
				<tr><td>€£Ұ₴$₰¢₤¥₳₲₪₵元₣₱฿¤₡₮₭₩ރ円₢₥₫₦zł﷼₠₧₯₨Kčर₹ƒ₸￠</td></tr>
				<tr><th id=""jiantoufuhao"">箭头符号<span id=""rtop""><a href=""#top"">↑返回顶部</a></span></th></tr>
				<tr><td>↑↓←→↖↗↘↙↔↕➻➼➽➸➳➺➻➴➵➶➷➹▶►▷◁◀◄«»➩➪➫➬➭➮➯➱⏎➲➾➔➘➙➚➛➜➝➞➟➠➡➢➣➤➥➦➧➨↚↛↜↝↞↟↠↠↡↢↣↤↤↥↦↧↨⇄⇅⇆⇇⇈⇉⇊⇋⇌⇍⇎⇏⇐⇑⇒⇓⇔⇖⇗⇘⇙⇜↩↪↫↬↭↮↯↰↱↲↳↴↵↶↷↸↹☇☈↼↽↾↿⇀⇁⇂⇃⇞⇟⇠⇡⇢⇣⇤⇥⇦⇧⇨⇩⇪↺↻⇚⇛♐</td></tr>				<tr><th id=""fuhaotuan"">符号图案<span id=""rtop""><a href=""#top"">↑返回顶部</a></span></th></tr>
                <tr><td>✐✎✏✑✒✍✉✁✂✃✄✆✉☎☏☑✓✔√☐☒✗✘ㄨ✕✖✖☢☠☣✈★☆✡囍㍿☯☰☲☱☴☵☶☳☷☜☞☝✍☚☛☟✌♤♧♡♢♠♣♥♦☀☁☂❄☃♨웃유❖☽☾☪✿♂♀✪✯☭➳卍卐√×■◆●○◐◑✙☺☻❀⚘♔♕♖♗♘♙♚♛♜♝♞♟♧♡♂♀♠♣♥❤☜☞☎☏⊙◎☺☻☼▧▨♨◐◑↔↕▪▒◊◦▣▤▥▦▩◘◈◇♬♪♩♭♪の★☆→あぃ￡Ю〓§♤♥▶¤✲❈✿✲❈➹☀☂☁【】┱┲❣✚✪✣✤✥✦❉❥❦❧❃❂❁❀✄☪☣☢☠☭ღ▶▷◀◁☀☁☂☃☄★☆☇☈⊙☊☋☌☍ⓛⓞⓥⓔ╬『』∴☀♫♬♩♭♪☆∷﹌の★◎▶☺☻►◄▧▨♨◐◑↔↕↘▀▄█▌◦☼♪の☆→♧ぃ￡❤▒▬♦◊◦♠♣▣۰•❤•۰►◄▧▨♨◐◑↔↕▪▫☼♦⊙●○①⊕◎Θ⊙¤㊣★☆♀◆◇◣◢◥▲▼△▽⊿◤◥✐✌✍✡✓✔✕✖♂♀♥♡☜☞☎☏⊙◎☺☻►◄▧▨♨◐◑↔↕♥♡▪▫☼♦▀▄█▌▐░▒▬♦◊◘◙◦☼♠♣▣▤▥▦▩◘◙◈♫♬♪♩♭♪✄☪☣☢☠♯♩♪♫♬♭♮☎☏☪♈ºº₪¤큐«»™♂✿♥　◕‿-｡　｡◕‿◕｡</td></tr>
				<tr><th id=""xilazimu"">希腊字母<span id=""rtop""><a href=""#top"">↑返回顶部</a></span></th></tr>
				<tr><td>ΑΒΓΔΕΖΗΘΙΚΛΜΝΞΟΠΡΣΤΥΦΧΨΩαβγδεζνξοπρσηθικλμτυφχψω</td></tr>
				<tr><th id=""eyuzimu"">俄语字母<span id=""rtop""><a href=""#top"">↑返回顶部</a></span></th></tr>
                <tr><td>АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя</td></tr>
				<tr><th id=""hanyupinyin"">汉语拼音<span id=""rtop""><a href=""#top"">↑返回顶部</a></span></th></tr>
				<tr><td>āáǎàōóǒòēéěèīíǐìūúǔùǖǘǚǜüêɑńňɡㄅㄆㄇㄈㄉㄊㄋㄌㄍㄎㄏㄐㄑㄒㄓㄔㄕㄖㄗㄘㄙㄚㄛㄜㄝㄞㄟㄠㄡㄢㄣㄤㄥㄦㄧㄨㄩ</td></tr>
				<tr><th id=""zhongwenzifu"">中文字符<span id=""rtop""><a href=""#top"">↑返回顶部</a></span></th></tr>
				<tr><td>零壹贰叁肆伍陆柒捌玖拾佰仟万亿吉太拍艾分厘毫微卍卐卄巜弍弎弐朤氺曱甴囍兀々〆のぁ〡〢〣〤〥〦〧〨〩㊎㊍㊌㊋㊏㊚㊛㊐㊊㊣㊤㊥㊦㊧㊨㊒㊫㊑㊓㊔㊕㊖㊗㊘㊜㊝㊞㊟㊠㊡㊢㊩㊪㊬㊭㊮㊯㊰㊀㊁㊂㊃㊄㊅㊆㊇㊈㊉</td></tr>
				<tr><th id=""riyu"">日文平假名片假名<span id=""rtop""><a href=""#top"">↑返回顶部</a></span></th></tr>
				<tr><td>ぁあぃいぅうぇえぉおかがきぎくぐけげこごさざしじすずせぜそぞただちぢっつづてでとどなにぬねのはばぱひびぴふぶぷへべぺほぼぽまみむめもゃやゅゆょよらりるれろゎわゐゑをんゔゕゖァアィイゥウェエォオカガキギクグケゲコゴサザシジスズセゼソゾタダチヂッツヅテデトドナニヌネノハバパヒビピフブプヘベペホボポマミムメモャヤュユョヨラリルレロヮワヰヱヲンヴヵヶヷヸヹヺ・ーヽヾヿ゠ㇰㇱㇲㇳㇴㇵㇶㇷㇸㇹㇺㇻㇼㇽㇾㇿ</td></tr>
				<tr><th id=""zhibiaofu"">制表符<span id=""rtop""><a href=""#top"">↑返回顶部</a></span></th></tr>
<tr><td>─ ━│┃╌╍╎╏┄ ┅┆┇┈ ┉┊┋┌┍┎┏┐┑┒┓└ ┕┖┗ ┘┙┚┛├┝┞┟┠┡┢┣ ┤┥┦┧┨┩┪┫┬ ┭ ┮ ┯ ┰ ┱ ┲ ┳ ┴ ┵ ┶ ┷ ┸ ┹ ┺ ┻┼ ┽ ┾ ┿ ╀ ╁ ╂ ╃ ╄ ╅ ╆ ╇ ╈ ╉ ╊ ╋ ╪ ╫ ╬═║╒╓╔ ╕╖╗╘╙╚ ╛╜╝╞╟╠ ╡╢╣╤ ╥ ╦ ╧ ╨ ╩ ╳╔ ╗╝╚ ╬ ═ ╓ ╩ ┠ ┨┯ ┷┏ ┓┗ ┛┳ ⊥ ﹃ ﹄┌ ╮ ╭ ╯╰</td></tr>
<tr><th id=""emoji"">emoji大全<span id=""rtop""><a href=""#top"">↑返回顶部</a></span></th></tr>
<tr><td><p id=""emoji1"">✌✋👌👍👊👋👏✍👣👀👂👃👅👄💋👑💍🌂⚽⚾🎮🎲🎷🎸🎺🎻🚲⌚⏰📻☎💰💳♠♥♦♣💯💘❤💓💔💕💖💗💙💚💛💜💝💞💟❣📱📶📳😂😃😄😅😆😋😎😍&zwj;<a href=""/emoji.html#emojidaquan"">更多...</a></p></td></tr>
<tr><th id=""huangguanfuhao"">皇冠符号(国际象棋)<span id=""rtop""><a href=""#top"">↑返回顶部</a></span></th></tr>
<tr><td>♚　♛　♝　♞　♜　♟　♔　♕　♗　♘　♖　♟</td></tr>
			</tbody></table>

        </dd>
      </dl>

    </div>
    <!-- /listbox -->
  </div>
</div></body></html>";
		}
    }
}
