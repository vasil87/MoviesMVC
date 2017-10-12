  $(document).ready(function(){
    $('.materialboxed').materialbox();
  });
  
  $(document).on('click','h6.collapsable-comment', function (event) {
      var element = event.target ;
      $(element).toggleClass('truncate');
  });
  
  $(document).on('click','#add-comment', function () {
      if ($('#comment-add').length > 0)
      {
          return false;
      }
      let $ul = $('#movie-comments');
      let imgSrc = $('#UserImgUrl').val();
      let user = $('#UserName').val();

      let elementToAdd = $(`<li class="collection-item comment avatar" id="comment-li" data-user="${user}">
                                <div class ="row">
                                   <div class ="col s8">
                                         <img src="${imgSrc}" width="50" class ="circle">
                                         <div class ="card">
                                            <input id="comment-add" type="textbox" >
                                          </div>
                                    </div>
                                    <div class ="col s2">
                                        <a id="confirm-comment" class ="btn-floating secondary-color right" title="Add"><i class ="material-icons">save</i></a>
                                   </div>
                                    <div class ="col s2">
                                        <a id="confirm-comment" class ="btn-floating secondary-color right stop-comment" title="Add"><i class ="material-icons">clear</i></a>
                                   </div>
                                </div>
                            </li>`)

      $ul.append(elementToAdd);

      $('html, body').animate({
          scrollTop: $(elementToAdd).offset().top
      }, 2000);

      $('#confirm-comment').on('click', () => {

          let saveCommentUrl = $('#SaveCommentUrl').val();
          let movieID = $('#MovieId').val();
          let text = $('#comment-add').val().trim();

          if(typeof text=== 'undefined' || text==='')
          {
              Materialize.toast('Can`t save empty comment',1000);
              return false;
          }
          //$('#loader').show();
          $.ajax({
              url: saveCommentUrl,
              type: "POST",
              data: JSON.stringify({ Comment: text, UserName: user, MovieId: movieID }),
              contentType: "application/json; charset=utf-8",
          }).done(function () {

              let urlForPartial = $('#RenderPartialComments').val();

              $.get(urlForPartial, function (result) {

                  $('#comments-container').replaceWith(result);
                  //$('#loader').hide();
                  Materialize.toast('New comment added',1000);
              })
          }).fail((errmessage) => {
              //$('#loader').hide();
              Materialize.toast(statusText, 1000);
          });
      })
  });

  $(document).on('click', '.delete-comment', function (ev) {
      let target = $(ev.target).parent();
      let id = $($(ev.target).siblings('.comment-id')[0]).val();
      let url = $('#DeletCommentUrl').val();
      let userName = $('#UserName').val();

      $.ajax({
          url: url,
          type: "POST",
          data: JSON.stringify({ commentId: id, userName: userName }),
          contentType: "application/json; charset=utf-8",
      }).done(function () {
          let urlForPartial = $('#RenderPartialComments').val();

          $.get(urlForPartial, function (result) {

              $('#comments-container').replaceWith(result);
              //$('#loader').hide();
              Materialize.toast('Comment removed succesfully', 1000);
          })
      }).fail((errmessage) => {
          //$('#loader').hide();
          Materialize.toast(statusText, 1000);
      });


  });

  $(document).on('click', '.stop-comment', function (ev) {
      $('#comment-li').remove();
  });

  var likeOrDislike = (urlToSend, isItLike) => {

      let movieID = $('#MovieId').val();
      let userName = $('#UserName').val();
    
      $.ajax({
          url: urlToSend,
          type: "POST",
          data: JSON.stringify({ MovieId: movieID, UserName: userName }),
          contentType: "application/json; charset=utf-8",
      }).done(function () {
          if (isItLike) {
              let current = Number($('#like-numbers').text());
              current += 1;
              $('#like-numbers').text(current);
              Materialize.toast('LikedSuccessfully', 1000);
          } else {
              let current = Number($('#dislike-numbers').text());
              current += 1;
              $('#dislike-numbers').text(current);
              Materialize.toast('Disliked successfully', 1000);
          }
      }).fail((errmessage) => {
          Materialize.toast(errmessage.statusText, 1000);
      });
  }

  $("#like-movie").on("click", function () {
      let url = $('#LikeAMovieUrl').val();
      likeOrDislike(url,true)
  });

  $("#dislike-movie").on("click", function() {
      let url=$('#DislikeAMovieUrl').val();
      likeOrDislike(url,false)
  });

