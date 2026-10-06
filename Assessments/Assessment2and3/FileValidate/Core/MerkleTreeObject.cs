namespace FileValidate.Core;

// <summary>
// MerkleTreeObject
// Will represent an implementation of a merkle tree
// </summary>
public interface IMerkleTreeObject
{	

	// <summary>
	// Gets the left subtree from the root of the tree
	// If it is a leaf node, it should return null
	// </summary>
	IMerkleTreeObject? GetLeftSubTree();


	// <summary>
	// Gets the right subtree from the root of the tree
	// If it is a leaf node, it should return null
	// </summary>
	IMerkleTreeObject? GetRightSubTree();

	// <summary>
	// Lists all the expected hashes (not computed)
	// that are within the file.
	// </summary>
	List<string> AllExpectedHashes();

	// <summary>
	// The list of hashes represented as strings that should represent
	// the completion state.
	//
	// 
	// Examples using a file with 4 chunks and 7 nodes (3 non-leaf nodes):
	//   1. If the file is complete, the root hash is return
	//   2. If the file's first two chunks are complete, the root's left child is returned
	//   3. If we have the first and last chunk complete, the first and last chunk hashes would be returned
	//   4. If the last half of the file is completed, the has of the root's right child is returned.
	// </summary>
	List<string> MinimumSetOfHashes_RepresentingCompletion();


	// <summary>
	// Retrieves the data that corresponds to the hash specified.
	//   1. If the hash corresponds to a leaf node, this will simply return the data related to that node
	//   2. If the hash corresponds to a non-leaf node,
	//          this will return the data of all leaf nodes within that subtree
	// 
	// </summary>
	List<byte> DataFromHash(string hash);

}
