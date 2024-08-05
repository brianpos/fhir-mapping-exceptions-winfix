using LibGit2Sharp;

Console.WriteLine("FHIR Core Specification mapping-exceptions.xml re-writer");

var repoRootPath = System.IO.Directory.GetCurrentDirectory();

// Before we start messing with things, lets ensure that this is actually the FHIR repo
// and we have those files checked out
using (var repo = new Repository(repoRootPath))
{
	// Ensure that this is the FHIR Core spec repo
	if (!repo.Network.Remotes.Any(r => r.Url == "https://github.com/HL7/fhir.git"))
	{
		Console.WriteLine($"This directory does not appear to contain a FHIR Core specification git repository.");
		return -1;
	}

	var files = System.IO.Directory.GetFiles(System.IO.Path.Combine(repoRootPath, "source"), "*-mapping-exceptions.xml", SearchOption.AllDirectories);
	foreach (var filename in files)
	{
		// Re-write the file to change over the line endings
		var lines = System.IO.File.ReadAllLines(filename);

		var fileGetStatus = repo.RetrieveStatus(filename);
		if (fileGetStatus == FileStatus.ModifiedInIndex || fileGetStatus == FileStatus.ModifiedInWorkdir)
		{
			Console.WriteLine($"correcting line endings for {filename} {fileGetStatus}");
			System.IO.File.WriteAllLines(filename, lines);
		}
	}
}
return 0;