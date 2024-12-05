using System.Data;
using AnsiCodes;

namespace day5
{
    class Update(IEnumerable<int> pages)
    {
        public int[] Pages { get; set; } = pages.ToArray();

        public int MiddlePage => Pages[Pages.Length / 2];

        public bool IsCorrectOrder(List<Rule> rules)
        {
            if (rules.Exists(r =>
            {
                var beforeIndex = Array.FindIndex(Pages, p => p == r.Before);
                var afterIndex = Array.FindIndex(Pages, p => p == r.After);

                if (beforeIndex != -1 && afterIndex != -1 && beforeIndex > afterIndex)
                {
                    return true;
                }
                return false;
            }))
            {
                return false;
            }
            return true;
        }

        public Update CorrectOrder(List<Rule> rules)
        {

            //Use only relevant rules
            List<Rule> updateRules = Pages.SelectMany(p => rules.Where(r => r.Before == p && Pages.Contains(r.After))).ToList();

            List<int> pagesToCheck = [.. Pages];

            List<int> inOrder = [];

            //For each page, we check if any still relevant rules have it as an 'after', meaning that another (unchecked) page is required before
            //If that is the case we move the page at the end of the list, to be re-checked when every other pages had a go
            //If we can't find any rules mentioning the page, we are free to place it in the result and we remove it **and any rule that have the page as a 'before'**
            while (pagesToCheck.Count > 0)
            {
                if (!updateRules.Exists(r => r.After == pagesToCheck[0]))
                {
                    inOrder.Add(pagesToCheck[0]);
                    updateRules.RemoveAll(r => r.Before == pagesToCheck[0]);
                    pagesToCheck.RemoveAt(0);
                }
                else
                {
                    pagesToCheck.Add(pagesToCheck[0]);
                    pagesToCheck.RemoveAt(0);
                }
            }

            return new Update(inOrder);
        }

        public override string ToString()
        {
            return string.Join(" ", Pages);
        }

        public string ToPrettyString(Update? baseUpdate = null)
        {
            return baseUpdate == null
                ? $"{Color.Green} \u2714 {Color.Default}{ToString()}"
                : $"{Color.Red}\u274C {Color.Default}{string.Join(" ", Pages.Select((p, i)
                    => $"{(p != baseUpdate.Pages[i] ? Color.Green : string.Empty)}{p}{Color.Default} "))}";
        }
    }
}