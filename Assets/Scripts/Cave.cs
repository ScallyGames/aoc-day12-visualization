using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cave
{
    public string Name { get; private set; }

    public List<Cave> AdjacentCaves = new List<Cave>();
    
    public GameObject GameObject { get; set; }

    public Cave(string name)
    {
        this.Name = name;
    }

    public bool IsLargeCave => this.Name.ToLower() != this.Name;

    public List<List<Cave>> GetAllPathsTo(string targetName, List<Cave> previousPath)
    {
        previousPath.Add(this);

        List<List<Cave>> paths = new List<List<Cave>>();

        if(this.Name == targetName)
        {
            paths.Add(previousPath);
            return paths;
        }

        var pathCandidates = this.AdjacentCaves.Where(x => IsValidCandiateForPath(x, previousPath));

        foreach(var candidate in pathCandidates)
        {
            List<Cave> newPath = new List<Cave>(previousPath);

            paths.AddRange(candidate.GetAllPathsTo(targetName, newPath));
        }

        return paths;
    }

    private bool IsValidCandiateForPath(Cave candidate, List<Cave> previousPath)
    {
        if(candidate.Name == previousPath[0].Name) return false;

        return candidate.IsLargeCave || !previousPath.Contains(candidate) || (!previousPath.GroupBy(x => x).Any(x => !x.Key.IsLargeCave && x.Count() > 1));
    }
}