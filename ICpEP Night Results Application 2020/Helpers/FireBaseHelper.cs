using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using System.Windows.Threading;

namespace ICpEP_Night_Results_Application_2020
{
    public class FireBaseHelper
    {
        FirebaseClient firebaseClient;
        public List<Vote> voted;
        MainWindow resultListenerMainWindow;

        public FireBaseHelper()
        {
            firebaseClient = new FirebaseClient("https://icpep-night-votingdb.firebaseio.com/", new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult("Fl3NjpC7oowMD2YZ8Nor6M3rfBnv0Pbumz7I01wR")
            });

            voted = new List<Vote>();
        }

        public async Task<bool> isConnected()
        {
            try
            {
                var response = await firebaseClient.Child("connected").OnceSingleAsync<String>();
                return bool.Parse(response);
            }
            catch (Exception e) { return false; }
        }

        public int totalVotes;
        public int[] votesM = new int[3];
        public int[] votesF = new int[3];
        public void startVotedListener(MainWindow mainWindow)
        {
            resultListenerMainWindow = mainWindow;

            totalVotes = 0;

            votesM[0] = 0;
            votesM[1] = 0;
            votesM[2] = 0;

            votesF[0] = 0;
            votesF[1] = 0;
            votesF[2] = 0;

            var observable = firebaseClient.Child("Voted").AsObservable<Vote>().Subscribe(d => VotedObserver(d));
            Console.WriteLine("Voted Listener | Listener started");
        }
        
        private void VotedObserver(FirebaseEvent<Vote> data)
        {
            if (data.Key != null && !data.Key.Equals(""))
            {
                bool doesExists = false;
                if (voted.Count > 0)
                {
                    foreach (var vote in voted)
                    {
                        if (vote.ID == data.Object.ID)
                        {
                            doesExists = true;
                        }
                    }
                }
                if (doesExists == false)
                {
                    Vote this_vote = new Vote(data.Object.ID, data.Object.VoteMale, data.Object.VoteFemale);
                    Console.WriteLine("Voted Observer | Added to voted: {0}", this_vote.ID);
                    voted.Add(this_vote);

                    totalVotes++;

                    switch (this_vote.VoteMale)
                    {
                        case 1920987:
                            votesM[0]++;
                            break;
                        case 1820826:
                            votesM[1]++;
                            break;
                        case 1520293:
                            votesM[2]++;
                            break;
                        default:
                            Console.WriteLine("Voted Observer | VoteMale ID Not Found: {0}", this_vote.VoteMale);
                            break;
                    }
                    switch (this_vote.VoteFemale)
                    {
                        case 1921078:
                            votesF[0]++;
                            break;
                        case 1820364:
                            votesF[1]++;
                            break;
                        case 1520045:
                            votesF[2]++;
                            break;
                        default:
                            Console.WriteLine("Voted Observer | VoteFemale ID Not Found: {0}", this_vote.VoteFemale);
                            break;
                    }
                }
            }
        }
    }
}
