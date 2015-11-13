# CrackerJac
MD5 Hash Cracking Software in C#

CrackerJac is a simple yet effective program designed to crack MD5 hashes that
are both unsalted and salted in MyBB style Md5(Md5(salt) + Md5(string)).

CrackerJac has two main modes: Dictionary attack and brute force. Dictionary
attack works by trying each entry in the dictionary against the specified hash.
Brute force attack is a method of last resort, where every possible character up
to a certain length is tried against the hash.

### Hash Files
Hash files are the files that contain the hashes you are trying to break. The syntax
is either of the following:
```
<name> <md5_hash>
```
```
<name> <md5_hash> <salt>
```

You can have multiple entries in your hash file, seperated by a \n character, with
comments in the form of:
```
<name> <md5_hash>
# This is a comment
<name> <md5_Hash>
```

### Dictionary Files
Dictionary files are files that contain the words that a dictionary attack will use
to try and break the hash. Each word is sperated by a \n character, giving it the format:
```
<word>
<word>
<word>
```

In the CrackerJac GitHub there is two sample dictionaries which work fairly well. There
is of course the Crackstation dictionaries, which are much longer but will take more time
to sort through,

## Dictionary Attack Mode
Dictionary Attack mode is enabled by default, and requires no special command line
parameters. To preform a simple dictionary attack run the command:
```
CrackerJac <hash_file.txt> <dictionary_file.txt>
```

CrackerJac will automatically attempt to break each hash in the hash file using each word
in the dictionary file. CrackerJac spawns a task (essentially a thread) for each hash
in the file.

### Advanced Modes
Dictionary attack mode by default checks the entry and only the entry in the dictionary file
against the hash. Advanced mode will start by appending the numbers 0-specified number to
the end of the entry and trying that against the hash. For example if the hash was for
'laptop123' and the dictionary only contained an entry for 'laptop', CrackerJac in regular
mode would not be able to crack it. But in advanced mode it will start appending 1-2-3-etc
to the end of laptop until it finds the result.

You must specify the upper bound for the advanced mode with the following syntax:
```
CrackerJac -a <upper_bound> <hash_file.txt> <dictionary_file.txt>
```

The upper_bound can be any positive integer greater than 0.

Another advanced mode is caps mode, where the normal dictionary entry of a hash is tried,
and then tried again with the same entry with a capitalized first letter. If the hash was
for 'Laptop' and the dictionary entry was 'laptop', -c mode would try 'Laptop', finding
the result.

The syntax for a caps mode is:
```
CrackerJac -c <hash_file.txt> <dictionary_file.txt>
```

To combine both modes together, say for the hash 'Laptop123', you would do:
```
CrackerJac -c -a 500 <hash_file.txt> <dictionary_file.txt>
```

### MyBB Salted Hash Mode
This is a flag you can enable to have CrackerJac use the MyBB-style method of hashing passwords,
which is Md5(Md5(salt) + Md5(password)). In your hash file you must use the second syntax, which is:
```
<name> <md5_hash> <salt>
```

The MyBB mode can be combined with any other modes of dictionary attack, for example:
```
CrackerJac -m -c -a 500 <hash_file.txt> <dictionary_file.txt>
```

## Brute Force Mode
Brute force is often regarded as the last option, brute force works by trying every single
character possible in order to try and find the hash. It is unlikely that a brute force
will be successful for days if not weeks.

When running a brute force attack you specify the max amount of characters you want the generated
string to be before it gives up. The syntax is:
```
CrackerJac -b <max_length> <hash_file.txt>
```

## Other Features
CrackerJac comes with hashing features that are useful for debugging and testing. For instance
you can use the hash generation methods to create a hash like so:
```
CrackerJac --generate-unsalted <string>
CrackerJac --generate-salted <string> <salt>
```

CrackerJac also comes with a way for you to search through your dictionary file and determine
if a dictionary entry is in it. Syntax is:
```
CrackerJac -s <query> <dictionary_file.txt>
```
