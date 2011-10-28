HR Auto - For PGP Encrypting ANSI 834 Files and SFTP'ing them to a Target Server
================================================================================

Overview
--------
This "HRAuto" dealie was a project I had written a long time ago that I am pretty much only using for a sample to stretch out my GIT legs with. It was designed to run on a secure internal corporate network as an intranet site application to assist with a common Human Resource task.  It is NOT an example of how to do this kind of stuff and wouldn't be very secure for the web at large or running in a shared insecure hosted environment. But...this is what it was:

It can take a set of ANSI 834 files (which were created by a different application on a network reachable drive in my case), encrypts them against a that file's target PGP file (probably provided by a vendor), then creates a .bat file on the fly, ProcessStartInfo() a hidden command prompt, and transmits the file SFTP to the target server leveraging the WINSCP application (which would have to be installed on that machine). Then it stores pertinent records into a database, cleans up any sensitive files, and then notifies the user of their success or failure. OBVIOUSLY, your web application would have to have very high trust, but like I said, this thing was designed to run in a very secure environment and a lot of assumptions were made about that when writing the project out.

This was originally written in an early release of VS 2008, I think and there was a lot of proprietary information in the application. All I did was open it up in VS 2010, strip out anything sensitive and replace files with dummy holders, and check it into GITHUB.

If I wrote this now I would write it WAAAAAAAY different and refactor the hell out of this thing (I *might* still if I get time), but I do know this much, and it's the funny thing about silly little software projects like this...it's been working and running for years where the production version lives...pretty much as you see it here.


What Do You Need to Run This?
-----------------------------
 * Well, the server needs to have WinSCP (http://winscp.net/eng/index.php) and if you don't know your way around the application, learn it.
 * Some understanding of Bouncy Castle (http://www.bouncycastle.org/csharp/) and Open PGP
 * Be desperate in figuring out how to SFTP some PGP'd files, because seriously, while this will work...it is full of pitfalls in its approach.