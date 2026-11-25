using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
public interface IQuestionProvider
{
    Task<TriviaQuestion> GetQuestionAsync(int difficulty);
}