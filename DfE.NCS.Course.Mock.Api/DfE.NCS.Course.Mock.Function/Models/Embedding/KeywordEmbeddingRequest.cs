using System;
using System.Collections.Generic;
using System.Text;

namespace DfE.NCS.Course.Mock.Function.Models.Embedding;

/// <summary>
/// Model class representing a request for keyword embedding. This class contains properties for the batch ID and a list of keywords that are to be processed for embedding.
/// </summary>
public class KeywordEmbeddingRequest
{
    /// <summary>
    /// Gets or sets the batch ID associated with the keyword embedding request. This ID is used to group and track the processing of a specific set of keywords for embedding. It is initialized to an empty string by default.
    /// </summary>
    public string BatchId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of keywords that are to be processed for embedding. This property is a list of strings, where each string represents a keyword that will be used in the embedding process. It is initialized to an empty list by default.
    /// </summary>
    public List<string> Keywords { get; set; } = [];
}
