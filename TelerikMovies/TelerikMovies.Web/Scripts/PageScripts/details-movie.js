  $(document).ready(function(){
    $('.materialboxed').materialbox();
  });
  
  $(document).on('click','h6.collapsable-comment', function (event) {
      var element = event.target ;
      $(element).toggleClass('truncate');
  });
  
  $('#add-comment').on('click', function () {
      if ($('comment-add').length > 0)
      {
          return false;
      }
      let $ul = $('#movie-comments');
      let imgSrc = $('#UserImgUrl').val();
      let user = $('#UserName').val();

      let elementToAdd = $(`<li class="collection-item comment avatar" data-user="${user}">
                                <div class ="row">
                                   <div class ="col s10">
                                         <img src="${imgSrc}" width="50" class ="circle">
                                         <div class ="card">
                                         <input id="comment-add" type="textbox" >
                                   </div>
                                        </div>
                                        <div class ="col s2">
                                        <a id="confirm-comment" class ="btn-floating secondary-color right" title="Add"><i class ="material-icons">save</i></a>
                                   </div>
                                </div>
                            </li>`)

      $ul.append(elementToAdd);

      let saveCommentUrl = $('#SaveCommentUrl').val();
      let movieID = $('#MovieId').val();
      let text=$('#comment-add').val().trim();

      $('#confirm-comment').one('click', () => {
          if(typeof text=== 'undefined' || text==='')
          {
              Materialize.toast('Can`t save empty comment',1000);
              return false;
          }
          $('#loader').show();
          $.ajax({
              url: saveCommentUrl,
              type: "POST",
              data: JSON.stringify({ Comment: text, UserName: user, MovieId: movieID }),
              contentType: "application/json; charset=utf-8",
          }).done(function () {
              let urlForPartial = $('#RenderPartialComments').val();
              $.get('RenderPartialComments', function (result) {

                  $('#comments-container').replaceWith(result);
                  $('#loader').hide();
                  Materialize.toast('New comment added');
              })
          });
      })
  });


  //$('#movies-data-table-admin').DataTable()
  //            .draw();
  //Materialize.toast('Succesfully removed', 3000)
  //$('#loader').hide();