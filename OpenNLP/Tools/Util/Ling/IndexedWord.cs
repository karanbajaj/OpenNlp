﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNLP.Tools.Util.Ling
{
    /**
 * This class is mainly for use with RTE in terms of the methods it provides,
 * but on a more general level, it provides a {@link CoreLabel} that uses its
 * DocIDAnnotation, SentenceIndexAnnotation, and IndexAnnotation to implement
 * Comparable/compareTo, hashCode, and equals.  This means no other annotations,
 * including the identity of the word, are taken into account when using these
 * methods.
 * <br>
 * The actual implementation is to wrap a <code>CoreLabel<code/>.
 * This avoids breaking the <code>equals()</code> and
 * <code>hashCode()</code> contract and also avoids expensive copying
 * when used to represent the same data as the original
 * <code>CoreLabel</code>.
 *
 * @author rafferty
 *
 */
    public class IndexedWord:IComparable<IndexedWord>
    {
        private static readonly long serialVersionUID = 3739633991145239829L;

  /**
   * The identifier that points to no word.
   */
  public static readonly IndexedWord NO_WORD = new IndexedWord(null, -1, -1);

  //private readonly CoreLabel label;
  private readonly Dictionary<object, object> label;

  /**
   * Default constructor; uses {@link CoreLabel} default constructor
   */
  public IndexedWord() {
    //label = new CoreLabel();
    label = new Dictionary<object, object>();
  }


  /**
   * Copy Constructor - relies on {@link CoreLabel} copy constructor
   * It will set the value, and if the word is not set otherwise, set
   * the word to the value.
   *
   * @param w A Label to initialize this IndexedWord from
   */
  /*public IndexedWord(Label w) {
    if (w is CoreLabel) {
      this.label = (CoreLabel) w;
    } else {
      label = new CoreLabel(w);
      if (label.word() == null) {
        label.setWord(label.value());
      }
    }
  }*/

  /**
   * Construct an IndexedWord from a CoreLabel just as for a CoreMap.
   * <i>Implementation note:</i> this is a the same as the constructor
   * that takes a CoreMap, but is needed to ensure unique most specific
   * type inference for selecting a constructor at compile-time.
   *
   * @param w A Label to initialize this IndexedWord from
   */
  /*public IndexedWord(CoreLabel w) {
    label = w;
  }*/

  /**
   * Constructor for setting docID, sentenceIndex, and
   * index without any other annotations.
   *
   * @param docID The document ID (arbitrary string)
   * @param sentenceIndex The sentence number in the document (normally 0-based)
   * @param index The index of the word in the sentence (normally 0-based)
   */
  public IndexedWord(String docID, int sentenceIndex, int index) {
    label = new Dictionary<object, object>();
    //label = new CoreLabel();
    /*label.set(CoreAnnotations.DocIDAnnotation.class, docID);
    label.set(CoreAnnotations.SentenceIndexAnnotation.class, sentenceIndex);
    label.set(CoreAnnotations.IndexAnnotation.class, index);*/
      label.Add(/*CoreAnnotations.*/"DocIDAnnotation"/*.class*/, docID);
    label.Add(/*CoreAnnotations.*/"SentenceIndexAnnotation"/*.class*/, sentenceIndex);
    label.Add(/*CoreAnnotations.*/"IndexAnnotation"/*.class*/, index);
  }

  public IndexedWord makeCopy(int count) {
    CoreLabel labelCopy = new CoreLabel(label);
    IndexedWord copy = new IndexedWord(labelCopy);
    copy.setCopyCount(count);
    return copy;
  }

  /**
   * TODO: would be nice to get rid of this.  Only used in two places in RTE.  
   */
  //public CoreLabel backingLabel() { return label; }

  /*public <VALUE> VALUE get(Class<? extends TypesafeMap.Key<VALUE>> key) {
    return label.get(key);
  }

  public <VALUE> bool has(Class<? extends TypesafeMap.Key<VALUE>> key) {
    return label.has(key);
  }

  public <VALUE> bool containsKey(Class<? extends TypesafeMap.Key<VALUE>> key) {
    return label.containsKey(key);
  }

  public <VALUE> VALUE set(Class<? extends TypesafeMap.Key<VALUE>> key, VALUE value) {
    return label.set(key, value);
  }

  public <KEY extends TypesafeMap.Key<String>> String getString(Class<KEY> key) {
    return label.getString(key);
  }

  public <VALUE> VALUE remove(Class<? extends Key<VALUE>> key) {
    return label.remove(key);
  }

  public Set<Class<?>> keySet() {
    return label.keySet();
  }*/

  public int size() {
    //return label.size();
    return label.Count;
  }

  //@Override
  public String value() {
    return label.value();
  }

  //@Override
  public void setValue(String value) {
    label.setValue(value);
  }

  //@Override
  public String tag() {
    return label.tag();
  }

  //@Override
  public void setTag(String tag) {
    label.setTag(tag);
  }

  //@Override
  public String word() {
    return label.word();
  }

  //@Override
  public void setWord(String word) {
    label.setWord(word);
  }

  //@Override
  public String lemma() {
    return label.lemma();
  }

  //@Override
  public void setLemma(String lemma) {
    label.setLemma(lemma);
  }

  //@Override
  public String ner() {
    return label.ner();
  }

  //@Override
  public void setNER(String ner) {
    label.setNER(ner);
  }

  //@Override
  public String docID() {
    return label.docID();
  }

  //@Override
  public void setDocID(String docID) {
    label.setDocID(docID);
  }

  //@Override
  public int index() {
    return label.index();
  }

  //@Override
  public void setIndex(int index) {
    label.setIndex(index);
  }

  //@Override
  public int sentIndex() {
    return label.sentIndex();
  }

  //@Override
  public void setSentIndex(int sentIndex) {
    label.setSentIndex(sentIndex);
  }

  //@Override
  public String originalText() {
    return label.originalText();
  }

  //@Override
  public void setOriginalText(String originalText) {
    label.setOriginalText(originalText);
  }

  //@Override
  public int beginPosition() {
    return label.beginPosition();
  }

  //@Override
  public int endPosition() {
    return label.endPosition();
  }

  //@Override
  public void setBeginPosition(int beginPos) {
    label.setBeginPosition(beginPos);
  }

  //@Override
  public void setEndPosition(int endPos) {
    label.setEndPosition(endPos);
  }

  public int copyCount() {
    return label.copyCount();
  }

  public void setCopyCount(int count) {
    label.setCopyCount(count);
  }

  public String toPrimes() {
    int copy = label.copyCount();
    return StringUtils.repeat('\'', copy);    
  }

  /**
   * This .equals is dependent only on docID, sentenceIndex, and index.
   * It doesn't consider the actual word value, but assumes that it is
   * validly represented by token position.
   * All IndexedWords that lack these fields will be regarded as equal.
   */
  //@Override
  public override bool Equals(Object o) {
    if (this == o) return true;
    if (!(o is IndexedWord)) return false;

    //now compare on appropriate keys
    IndexedWord otherWord = (IndexedWord) o;
    int myInd = get(CoreAnnotations.IndexAnnotation.class);
    int otherInd = otherWord.get(CoreAnnotations.IndexAnnotation.class);
    if (myInd == null) {
      if (otherInd != null)
      return false;
    } else if ( ! myInd.Equals(otherInd)) {
      return false;
    }
    int mySentInd = get(CoreAnnotations.SentenceIndexAnnotation.class);
    int otherSentInd = otherWord.get(CoreAnnotations.SentenceIndexAnnotation.class);
    if (mySentInd == null) {
      if (otherSentInd != null)
      return false;
    } else if ( ! mySentInd.Equals(otherSentInd)) {
      return false;
    }
    String myDocID = getString(CoreAnnotations.DocIDAnnotation.class);
    String otherDocID = otherWord.getString(CoreAnnotations.DocIDAnnotation.class);
    if (myDocID == null) {
      if (otherDocID != null)
      return false;
    } else if ( ! myDocID.Equals(otherDocID)) {
      return false;
    }
    if (copyCount() != otherWord.copyCount()) {
      return false;
    }
    return true;
  }


  /**
   * This hashCode uses only the docID, sentenceIndex, and index.
   * See compareTo for more info.
   */
  //@Override
        public override int GetHashCode()
        {
    bool sensible = false;
    int result = 0;
    if (get(CoreAnnotations.DocIDAnnotation.class) != null) {
      result = get(CoreAnnotations.DocIDAnnotation.class).hashCode();
      sensible = true;
    }
    if (has(CoreAnnotations.SentenceIndexAnnotation.class)) {
      result = 29 * result + get(CoreAnnotations.SentenceIndexAnnotation.class).hashCode();
      sensible = true;
    }
    if (has(CoreAnnotations.IndexAnnotation.class)) {
      result = 29 * result + get(CoreAnnotations.IndexAnnotation.class).hashCode();
      sensible = true;
    }
    if ( ! sensible) {
      //System.err.println("WARNING!!!  You have hashed an IndexedWord with no docID, sentIndex or wordIndex. You will almost certainly lose");
    }
    return result;
  }

  /**
   * NOTE: This compareTo is based on and made to be compatible with the one
   * from IndexedFeatureLabel.  You <em>must</em> have a DocIDAnnotation,
   * SentenceIndexAnnotation, and IndexAnnotation for this to make sense and
   * be guaranteed to work properly. Currently, it won't error out and will
   * try to return something sensible if these are not defined, but that really
   * isn't proper usage!
   *
   * This compareTo method is based not by value elements like the word(),
   *  but on passage position. It puts NO_WORD elements first, and then orders
   *  by document, sentence, and word index.  If these do not differ, it
   *  returns equal.
   *
   *  @param w The IndexedWord to compare with
   *  @return Whether this is less than w or not in the ordering
   */
  public int CompareTo(IndexedWord w) {
    if (this.Equals(IndexedWord.NO_WORD)) {
      if (w.Equals(IndexedWord.NO_WORD)) {
        return 0;
      } else {
        return -1;
      }
    }
    if (w.Equals(IndexedWord.NO_WORD)) {
      return 1;
    }

    String docID = this.getString(CoreAnnotations.DocIDAnnotation.class);
    int docComp = docID.CompareTo(w.getString(CoreAnnotations.DocIDAnnotation.class));
    if (docComp != 0) return docComp;

    int sentComp = sentIndex() - w.sentIndex();
    if (sentComp != 0) return sentComp;

    int indexComp = index() - w.index();
    if (indexComp != 0) return indexComp;

    return copyCount() - w.copyCount();
  }

  /**
   * Returns the value-tag of this label.
   */
  //@Override
  public override String ToString() {
    return label.ToString(CoreLabel.OutputFormat.VALUE_TAG);
  }

  public String toString(CoreLabel.OutputFormat format) {
    return label.ToString(format);
  }

  /**
   * {@inheritDoc}
   */
  //@Override
  public void setFromString(String labelStr) {
    throw new InvalidOperationException("Cannot set from string");
  }


  public static LabelFactory factory() {
    return new LabelFactory() {

      public Label newLabel(String labelStr) {
        CoreLabel label = new CoreLabel();
        label.setValue(labelStr);
        return new IndexedWord(label);
      }

      public Label newLabel(String labelStr, int options) {
        return newLabel(labelStr);
      }

      public Label newLabel(Label oldLabel) {
        return new IndexedWord(oldLabel);
      }

      public Label newLabelFromString(String encodedLabelStr) {
        throw new UnsupportedOperationException("This code branch left blank" +
        " because we do not understand what this method should do.");
      }
    };
  }
  /**
   * {@inheritDoc}
   */
  //@Override
  public LabelFactory labelFactory() {
    return IndexedWord.factory();
  }
    }
}