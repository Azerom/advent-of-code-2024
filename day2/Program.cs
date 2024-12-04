using System.Text.RegularExpressions;

var pattern = @"(?>(?'level'\d+) ?)+";

IEnumerable<Report> reports;

using (var sr = new StreamReader("input.txt"))
{
    var matches = Regex.Matches(await sr.ReadToEndAsync(), pattern);

    reports = matches.Select(m => new Report { Levels = m.Groups["level"].Captures.Select(c => int.Parse(c.Value)).ToArray()});
}

Console.WriteLine($"Part 1 : {reports.Count(r => r.IsSafe)}");
Console.WriteLine($"Part 2 : {reports.Count(r => r.IsSafeWithDampener)}");

class Report{
    public required int[] Levels { get; set; }

    public bool IsSafe
    {
        get
        {
            bool? isGlobalDesc = null;

            for (int i = 1; i < Levels.Length; i++)
            {
                if(Levels[i] == Levels[i - 1]){
                    return false;
                }
                else{
                    if(Math.Abs(Levels[i] - Levels[i - 1]) > 3){
                        return false;
                    }

                    bool isDesc = Levels[i - 1] > Levels[i];

                    if(isGlobalDesc == null){
                        isGlobalDesc = isDesc;
                    }
                    else if(isDesc != isGlobalDesc){
                        return false;
                    }
                }
            }

            return true;
        }
    }

    public bool IsSafeWithDampener
    {
        get
        {
            return(IsReportSafeWithDampener2());
        }

    }

    private bool IsReportSafeWithDampener2(bool allowUnsafe = true, int[]? levels = null){

        levels ??= Levels;

        //Check if any direction possibly safe
        int nbAsc = 0;
        int nbDesc = 0;
        for (int i = 0; i < levels.Length - 1; i++){
            if(levels[i] < levels[i + 1])
            {
                nbAsc++;
            }
            else if(levels[i] > levels[i + 1]){
                nbDesc++;
            }
        }

        //Get true direction
        bool isAsc = nbAsc > nbDesc;

        if((isAsc && nbAsc < levels.Length - 2) || (!isAsc && nbDesc < levels.Length - 2)){
            return false;
        }

        List<int> unsafeIds = new();

        for (int i = 0; i < levels.Length - 1; i++)
        {
            bool isSafe = true;

            if(!IsCoupleSafe(levels[i], levels[i + 1], isAsc)){
                unsafeIds.Add(i);
            }
        }

        if(allowUnsafe && unsafeIds.Count > 0)
        {
            if(unsafeIds.Count > 2){
                return false;
            }

            if(unsafeIds.Count == 2 && unsafeIds[0] + 1 == unsafeIds[1]){ //Cas 1 9 2
                if(IsCoupleSafe(levels[unsafeIds[0]], levels[unsafeIds[0] + 2], isAsc)){
                    return true;
                }
                else{
                    return false;
                }
            }
            if(unsafeIds.Count == 1){
                if(unsafeIds[0] == 0 || unsafeIds[0] + 1 == levels.Length - 1){  // Cas 8 1 2 3 et 1 2 3 1
                    return true;
                }
                
                if( IsCoupleSafe(levels[unsafeIds[0]], levels[unsafeIds[0] + 2], isAsc)
                    // || ( IsCoupleSafe(levels[unsafeIds[0] + 1], levels[unsafeIds[0] + 2], isAsc) && IsCoupleSafe(levels[unsafeIds[0] - 1], levels[unsafeIds[0] + 1], isAsc) ) ){ // Cas 1 2 8 3 4

                    || IsCoupleSafe(levels[unsafeIds[0] - 1], levels[unsafeIds[0] + 1], isAsc) ){ // Cas 1 2 8 3 4

                    return true;
                }
            }

            return false;
        }

        return true;
    }

    private bool IsCoupleSafe(int first, int second, bool isAsc)
    {
        if(first == second || Math.Abs(first - second) > 3){
            return false;
        }
        if((isAsc && first > second) || (!isAsc && first < second)){
            return false;
        }
        return true;
    }

        private bool IsReportSafeWithDampener(bool? isGlobalDesc = null, int[] levels = null){

            bool ping = false;
            if(levels == null){
                levels = Levels;
                ping = true;
            }

            bool dampenerUsed = false;
            int lastLevel = levels[0];
            bool doRecheck = isGlobalDesc == null;

            for (int i = 1; i < levels.Length; i++)
            {
                bool isSafe = true;

                int diff = levels[i] - lastLevel;

                if(diff == 0 || Math.Abs(diff) > 3){
                    isSafe = false;
                }

                bool isDesc = diff < 0;

                if(isGlobalDesc == null){
                    isGlobalDesc = isDesc;
                }
                else if(isDesc != isGlobalDesc){
                    isSafe = false;
                }

                if(!isSafe){
                    if(dampenerUsed){
                        //Specifique pour le premier couple
                        if(doRecheck){
                            if(IsReportSafeWithDampener(!isGlobalDesc)){
                                return true;
                            }
                        }
                        
                        if(!ping){
                            int[] newLevels = (int[]) levels.Clone();
                            Array.Clear(newLevels, 1, 1);
                            if(IsReportSafeWithDampener(null, newLevels)){
                                return true;
                            }
                        }


                        return false;
                    }
                    dampenerUsed = true;
                }
                else{
                    lastLevel = levels[i];
                }
            }

            return true;
        }
}